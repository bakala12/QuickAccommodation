using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationShared.Dtos;
using AccomodationWebApi.Attributes;
using AccomodationWebApi.Providers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Threading;
using System.Transactions;
using System.Web.Http;

namespace AccomodationWebApi.Controllers
{
    [RoutePrefix("api/offers")]
    public class OffersController : ApiController
    {

        private readonly IContextProvider _provider;

        public OffersController(IContextProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            _provider = provider;
        }

        public OffersController()
        {
            _provider = new ContextProvider<AccommodationContext>();
        }


        /// <summary>
        /// Wysyła listę ofert użytkownika o danym id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("GetUserOffers/{UserId?}"), HttpGet]
        public IHttpActionResult GetUserOffers(int userId)
        {

            IList<Offer> ret;

            using (var context = _provider.GetNewContext())
            {
                using (var transaction = new TransactionScope())
                {
                    if (context is DbContext) (context as DbContext).Configuration.ProxyCreationEnabled = false;
                    ret = context.Offers.Where(o => o.VendorId == userId).ToList();
                    transaction.Complete();
                }
            }

            return Ok(ret);

        }

        /// <summary>
        /// Wysyła listę wszystkich (w tym także już nieaktualnych) ofert użytkowania.
        /// Potrzebne przy tworzeniu hostorii
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("GetUserHistoricalOffers/{UserId?}"), HttpGet]
        public IHttpActionResult GetUserHistoricalOffers(int userId)
        {
            IList<HistoricalOffer> ret;

            using (var context = _provider.GetNewContext())
            {
                using (var transaction = new TransactionScope())
                {
                    if (context is DbContext)
                        (context as DbContext).Configuration.ProxyCreationEnabled = false;
                    ret = context.HistoricalOffers.Where(o => o.VendorId == userId).ToList();
                    transaction.Complete();
                }
            }

            return Ok(ret);

        }

        /// <summary>
        /// Wysyła ofertę o danym id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetOffer/{id?}"), HttpGet]
        public IHttpActionResult Get(int id)
        {
            Offer offer = null;

            using (var context = _provider.GetNewContext())
            {
                using (var transaction = new TransactionScope())
                {
                    if (context is DbContext)
                        (context as DbContext).Configuration.ProxyCreationEnabled = false;
                    offer = context.Offers.FirstOrDefault(o => o.Id == id);
                    transaction.Complete();
                }
            }

            if (offer == null)
            {
                return (IHttpActionResult)NotFound();
            }
            return Ok(offer);


        }

        /// <summary>
        /// Wysyła ofertę o danym id (oferta może być już nieaktualna)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetHistoricalOffer/{id?}"), HttpGet]
        public IHttpActionResult GetHistorcialOffer(int id)
        {
            HistoricalOffer offer = null;

            using (var context = _provider.GetNewContext())
            {
                    if (context is DbContext) (context as DbContext).Configuration.ProxyCreationEnabled = false;
                    offer = context.HistoricalOffers.FirstOrDefault(o => o.OriginalOfferId == id);
            }

            if (offer == null)
            {
                return (IHttpActionResult)NotFound();
            }
            return Ok(offer);


        }
        /// <summary>
        /// Zapisuje nową ofertę w bazie
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("saveOffer"), HttpPost]
        [RequireHttps]
        public IHttpActionResult SaveOfferAsync(OfferAllDataDto dto)
        {
            using (var context = _provider.GetNewContext())
            {
                using (var transaction = new TransactionScope())
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

                    //zapisanie do historii
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
                    transaction.Complete();
                }
            }

            return Ok(true);
        }

        /// <summary>
        /// Edycja oferty w bazie
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("editOffer"), HttpPost]
        [RequireHttps]
        public IHttpActionResult EditOffer(OfferEditDataDto dto)
        {
            using (var context = _provider.GetNewContext())
            {
                using (var transaction = new TransactionScope())
                {
                    User user = context.Users.FirstOrDefault(u => u.Username.Equals(dto.Username));
                    if (user == null) return NotFound();
                    Offer offer = context.Offers.FirstOrDefault(x => x.Id == dto.OfferId);
                    if (offer == null) return NotFound();

                    HistoricalOffer ho = context.HistoricalOffers.FirstOrDefault(x => x.OriginalOfferId == offer.Id);

                    ho.OfferInfo = offer.OfferInfo = dto.OfferInfo;
                    ho.Vendor = offer.Vendor = user;
                    ho.Room = offer.Room = dto.Room;
                    ho.Room.Place = offer.Room.Place = dto.Place;
                    ho.Room.Place.Address = offer.Room.Place.Address = dto.Place.Address;

                    context.SaveChanges();
                    transaction.Complete();
                }
            }
            return Ok();
        }

        /// <summary>
        /// Usuuwanie oferty z bazy
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("removeOffer"), HttpPost]
        [RequireHttps]
        public IHttpActionResult RemoveOfferAsync(OfferEditDataDto dto)
        {

            using (var context = _provider.GetNewContext())
            {
                using (var transaction = new TransactionScope())
                {
                    User user = context.Users.FirstOrDefault(x => x.Username.Equals(dto.Username));
                    if (user == null) return NotFound();
                    Offer offer = context.Offers.FirstOrDefault(x => x.Id == dto.OfferId);
                    if (offer == null) return NotFound();

                    OfferInfo offerInfo = context.OfferInfo.FirstOrDefault(x => x.Id == offer.OfferInfoId);
                    Room room = context.Rooms.FirstOrDefault(x => x.Id == offer.RoomId);
                    Place place = context.Places.FirstOrDefault(x => x.Id == room.PlaceId);
                    Address address = context.Addresses.FirstOrDefault(x => x.Id == place.AddressId);

                    var ho = context.HistoricalOffers.FirstOrDefault(h => h.OriginalOfferId == offer.Id);
                    if (ho != null) ho.OriginalOffer = null;
                    //usuń z bazy ofertę oraz jej dane

                    context.Offers.Remove(offer);
                    user?.MyOffers?.Remove(offer);
                    context.SaveChanges();
                    transaction.Complete();
                }
            }
            return Ok();
        }

        /// <summary>
        /// Rezerwacja oferty
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("reserve"), HttpPost]
        public IHttpActionResult ReserveOffer(ReserveOfferDto dto)
        {
            using (var context = _provider.GetNewContext())
            {
                using (var transaction = new TransactionScope())
                {
                    Offer offer = context.Offers.FirstOrDefault(o => o.Id == dto.OfferId);

                    User customer = context.Users.FirstOrDefault(u => u.Username.Equals(dto.Username));
                    User vendor = context.Users.FirstOrDefault(x => x.Id == offer.VendorId);

                    OfferInfo offerInfo = context.OfferInfo.FirstOrDefault(o => o.Id == offer.OfferInfoId);
                    UserData customerData = context.UserData.FirstOrDefault(x => x.Id == customer.UserDataId);
                    UserData vendorData = context.UserData.FirstOrDefault(x => x.Id == vendor.UserDataId);
                    Room room = context.Rooms.FirstOrDefault(x => x.Id == offer.RoomId);
                    Place place = context.Places.FirstOrDefault(x => x.Id == room.PlaceId);

                    if (offer == null || customer == null) return NotFound();
                    if (offer.IsBooked) return BadRequest();
                    offer.IsBooked = true;
                    offer.Customer = customer;
                    context.SaveChanges();
                    //Wysłanie powiadomienia mailowego, ostatni parametr oznacza rezerwację
                    EmailNotification.SendNotification(offerInfo, place, vendorData, customerData, room,true);
                    transaction.Complete();
                }
            }


            return Ok(true);
        }

        /// <summary>
        /// Rezygnacja z oferty
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Route("resign"), HttpPost]
        public IHttpActionResult ResignOffer(ReserveOfferDto dto)
        {
            using (var context = _provider.GetNewContext())
            {
                using (var transaction = new TransactionScope())
                {
                    Offer offer = context.Offers.FirstOrDefault(o => o.Id == dto.OfferId);
                    User customer = context.Users.FirstOrDefault(u => u.Username.Equals(dto.Username));
                    User vendor = context.Users.FirstOrDefault(x => x.Id == offer.VendorId);

                    if (offer == null || customer == null) return NotFound();
                    if (!offer.IsBooked) return BadRequest();


                    OfferInfo offerInfo = context.OfferInfo.FirstOrDefault(o => o.Id == offer.OfferInfoId);
                    UserData customerData = context.UserData.FirstOrDefault(x => x.Id == customer.UserDataId);
                    UserData vendorData = context.UserData.FirstOrDefault(x => x.Id == vendor.UserDataId);
                    Room room = context.Rooms.FirstOrDefault(x => x.Id == offer.RoomId);
                    Place place = context.Places.FirstOrDefault(x => x.Id == room.PlaceId);
                    offer.IsBooked = false;
                    offer.Customer = null;
                    context.SaveChanges();
                    //Wysłanie powiadomienia mailowego, ostatni parametr oznacza rezygnację
                    EmailNotification.SendNotification(offerInfo, place, vendorData, customerData, room, false);
                    transaction.Complete();
                }
            }
            return Ok(true);
        }

        /// <summary>
        /// Wysyła zarezerwowane oferty danego usera
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [Route("reservedOffers/{username?}"), HttpGet]
        public IHttpActionResult GetReservedOffers(string username)
        {
            IList<Offer> offers = null;
            using (var context = _provider.GetNewContext())
            {
                    if (context is DbContext) (context as DbContext).Configuration.ProxyCreationEnabled = false;
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
