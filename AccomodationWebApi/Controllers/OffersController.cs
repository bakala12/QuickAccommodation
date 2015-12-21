using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationShared.Dtos;
using AccomodationWebApi.Attributes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Threading;
using System.Web.Http;

namespace AccomodationWebApi.Controllers
{
    //test implementation only
    [RoutePrefix("api/offers")]
    public class OffersController : ApiController
    {
        [Route("GetUserOffers/{UserId?}"), HttpGet]
        public IHttpActionResult GetUserOffers(int userId)
        {

            IList<Offer> ret;

            using (var context = new AccommodationContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                ret = context.Offers.Where(o => o.VendorId == userId).ToList();
            }

            return Ok(ret);

        }

        public IHttpActionResult Get(int id)
        {
            Offer offer = null;

            using (var context = new AccommodationContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                offer = context.Offers.FirstOrDefault(o => o.Id == id);
            }

            if (offer == null)
            {
                return (IHttpActionResult)NotFound();
            }
            return Ok(offer);


        }
        [Route("saveOffer"), HttpPost]
        [RequireHttps]
        public IHttpActionResult SaveOfferAsync(OfferAllDataDto dto)
        {
            using (var context = new AccommodationContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    Offer offerToAdd = new Offer();

                    User user = context.Users.FirstOrDefault(x => x.Id == dto.Vendor.Id);
                    if (user == null) return NotFound();

                    offerToAdd.Vendor = user;
                    offerToAdd.OfferInfo = dto.OfferInfo;

                    Place place = context.Places.FirstOrDefault(p => p.PlaceName.Equals(dto.Place.PlaceName) &&
                                                                     p.Address.City.Equals(dto.Place.Address.City) &&
                                                                     p.Address.Street.Equals(dto.Place.Address.Street) &&
                                                                     p.Address.LocalNumber == dto.Place.Address.LocalNumber);
                    if (place != null)
                    {
                        //istnieje to miejsce w bazie danych
                        Room room = context.Rooms.FirstOrDefault(r => r.PlaceId == place.Id && r.Number == dto.Room.Number);
                        if (room != null)
                        {
                            //istnieje oferta na ten pokój
                            List<Offer> off =
                                context.Offers.Where(offer => offer.RoomId == room.Id)
                                    .Include(o => o.OfferInfo)
                                    .ToList();
                            if (off.Any(offer => (offer.OfferInfo.OfferStartTime <= dto.OfferInfo.OfferStartTime &&
                                                  offer.OfferInfo.OfferEndTime >= dto.OfferInfo.OfferStartTime) ||
                                                 (offer.OfferInfo.OfferStartTime >= dto.OfferInfo.OfferStartTime &&
                                                  offer.OfferInfo.OfferEndTime <= dto.OfferInfo.OfferEndTime)))
                            {
                                return BadRequest();
                            }
                            //żadna oferta nie koliduje
                            offerToAdd.Room = room;
                        }
                        else
                        {
                            //nowy pokój
                            offerToAdd.Room = dto.Room;
                            offerToAdd.Room.Place = place;
                        }
                    }
                    else
                    {
                        offerToAdd.Room = dto.Room;
                        offerToAdd.Room.Place = dto.Place;
                        offerToAdd.Room.Place.Address = dto.Place.Address;
                    }
                    user.MyOffers.Add(offerToAdd);

                    HistoricalOffer historicalOffer = new HistoricalOffer();
                    historicalOffer.OfferInfo = offerToAdd.OfferInfo;
                    historicalOffer.Vendor = offerToAdd.Vendor;
                    historicalOffer.Room = offerToAdd.Room;
                    historicalOffer.Room.Place = offerToAdd.Room.Place;
                    historicalOffer.Room.Place.Address = offerToAdd.Room.Place.Address;
                    historicalOffer.OriginalOffer = offerToAdd;

                    user.MyHistoricalOffers.Add(historicalOffer);

                    //ewentualna zmiana rangi
                    int c = user.MyHistoricalOffers.Count;
                    if (c >= 4 && c < 8)
                    {
                        user.Rank = context.Ranks.FirstOrDefault(r => r.Name.Equals("Junior"));
                    }
                    else if (c >= 8 && c < 15)
                    {
                        user.Rank = context.Ranks.FirstOrDefault(r => r.Name.Equals("Znawca"));
                    }
                    else if (c >= 15 && c < 25)
                    {
                        user.Rank = context.Ranks.FirstOrDefault(r => r.Name.Equals("Mistrz"));
                    }
                    else if (c >= 25)
                    {
                        user.Rank = context.Ranks.FirstOrDefault(r => r.Name.Equals("Guru"));
                    }

                    context.SaveChanges();
                    transaction.Commit();
                }
            }

            return Ok();
        }

        [Route("editOffer"), HttpPost]
        [RequireHttps]
        public IHttpActionResult EditOffer(OfferEditDataDto dto)
        {
            using (var context = new AccommodationContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    User user = context.Users.FirstOrDefault(u => u.Username.Equals(dto.Username));
                    if (user == null) return NotFound();
                    Offer offer = context.Offers.FirstOrDefault(x => x.Id == dto.OfferId);
                    if (offer == null) return NotFound();

                    offer.OfferInfo = dto.OfferInfo;
                    offer.Vendor = user;
                    offer.Room = dto.Room;
                    offer.Room.Place = dto.Place;
                    offer.Room.Place.Address = dto.Place.Address;

                    context.SaveChanges();
                    transaction.Commit();
                }
            }
            return Ok();
        }

        [Route("removeOffer"), HttpPost]
        [RequireHttps]
        public IHttpActionResult RemoveOfferAsync(OfferEditDataDto dto)
        {

            using (var context = new AccommodationContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    User user = context.Users.FirstOrDefault(x => x.Username.Equals(dto.Username));
                    if (user == null) return NotFound();
                    Offer offer = context.Offers.FirstOrDefault(x => x.Id == dto.OfferId);
                    if (offer == null) return NotFound();

                    OfferInfo offerInfo = context.OfferInfo.FirstOrDefault(x => x.Id == offer.OfferInfoId);
                    Place place = context.Places.FirstOrDefault(x => x.Id == offer.Room.PlaceId);
                    Address address = context.Addresses.FirstOrDefault(x => x.Id == place.AddressId);

                    //usuń z bazy ofertę oraz jej dane
                    context.Offers.Remove(offer);
                    context.Places.Remove(place);
                    context.Addresses.Remove(address);
                    context.OfferInfo.Remove(offerInfo);
                    user.MyOffers.Remove(offer);

                    context.SaveChanges();
                    transaction.Commit();
                }
            }
            return Ok();
        }

        [Route("reserve"), HttpPost]
        public IHttpActionResult ReserveOffer(ReserveOfferDto dto)
        {
            using (var context = new AccommodationContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    Offer offer = context.Offers.FirstOrDefault(o => o.Id == dto.OfferId);
                    User user = context.Users.FirstOrDefault(u => u.Username.Equals(dto.Username));
                    if (offer == null || user == null) return NotFound();
                    if (offer.IsBooked) return BadRequest();
                    offer.IsBooked = true;
                    offer.Customer = user;
                    context.SaveChanges();
                    transaction.Commit();
                }
            }
            return Ok(true);
        }

        [Route("resign"), HttpPost]
        public IHttpActionResult ResignOffer(ReserveOfferDto dto)
        {
            using (var context = new AccommodationContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    Offer offer = context.Offers.FirstOrDefault(o => o.Id == dto.OfferId);
                    User user = context.Users.FirstOrDefault(u => u.Username.Equals(dto.Username));
                    if (offer == null || user == null) return NotFound();
                    if (!offer.IsBooked) return BadRequest();
                    offer.IsBooked = false;
                    offer.Customer = null;
                    context.SaveChanges();
                    transaction.Commit();
                }
            }
            return Ok(true);
        }

        [Route("reservedOffers/{username?}"), HttpGet]
        public IHttpActionResult GetReservedOffers(string username)
        {
            IList<Offer> offers = null;
            using (var context = new AccommodationContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                User user = context.Users.FirstOrDefault(u => u.Username.Equals(username));
                if (user == null) return NotFound();
                offers = context.Offers.Where(o => o.CustomerId == user.Id).ToList();
                foreach (var offer in offers)
                {
                    offer.Customer = null;
                }
            }
            return Ok(offers);

        }
    }
}
