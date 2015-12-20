using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationShared.Dtos;
using AccomodationWebApi.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

                    offerToAdd.OfferInfo = dto.OfferInfo;
                    offerToAdd.Vendor = user;
                    offerToAdd.Room = dto.Room;
                    offerToAdd.Room.Place = dto.Place;

                    user.MyOffers.Add(offerToAdd);

                    HistoricalOffer historicalOffer = new HistoricalOffer();
                    historicalOffer.Room = dto.Room;
                    historicalOffer.Room.Place = dto.Place;
                    historicalOffer.OfferInfo = dto.OfferInfo;
                    historicalOffer.Vendor = user;

                    user.MyHistoricalOffers.Add(historicalOffer);

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

                    //historyczną edytować?? dodać nową??

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
    }
}
