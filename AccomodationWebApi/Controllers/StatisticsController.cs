using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Channels;
using System.Web.Http;
using AccommodationDataAccess.Domain;
using AccomodationWebApi.Providers;
using System.Data.Entity;
using AccommodationDataAccess.Model;

namespace AccomodationWebApi.Controllers
{
    [RoutePrefix("api/Statistics")]
    public class StatisticsController : ApiController
    {
        private readonly IContextProvider _provider;

        public StatisticsController(IContextProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            _provider = provider;
        }

        public StatisticsController()
        {
            _provider = new ContextProvider<AccommodationContext>();
        }

        [Route("rank/{username?}"), HttpGet]
        public IHttpActionResult GetUserRankName(string username)
        {
            using (var context = _provider.GetNewContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username.Equals(username));
                if (user == null) return NotFound();
                string name = user.Rank.Name;
                return Ok(name);
            }
        }

        [Route("offersCount/{username?}"), HttpGet]
        public IHttpActionResult GetUsersOffersCount(string username)
        {
            using (var context = _provider.GetNewContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username.Equals(username));
                if (user == null) return NotFound();
                return Ok(user.MyOffers.Count);
            }
        }

        [Route("reservedOffersCount/{username?}"), HttpGet]
        public IHttpActionResult GetUsersReservedOffersCount(string username)
        {
            using (var context = _provider.GetNewContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username.Equals(username));
                if (user == null) return NotFound();
                return Ok(user.PurchasedOffers.Count);
            }
        }

        [Route("CheapestOfferPrice/{username?}"), HttpGet]
        public IHttpActionResult CheapestOfferPrice(string username)
        {
            using (var context = _provider.GetNewContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username.Equals(username));
                if (user == null) return NotFound();

                List<Offer> list = context.Offers.Where(x => x.VendorId == user.Id).Include(o => o.OfferInfo).ToList();
                double min = list.Min(x => x.OfferInfo.Price);
                return Ok(min);
            }
        }

        [Route("MostExpensiveOfferPrice/{username?}"), HttpGet]
        public IHttpActionResult MostExpensiveOfferPrice(string username)
        {
            using (var context = _provider.GetNewContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username.Equals(username));
                if (user == null) return NotFound();

                List<Offer> list = context.Offers.Where(x => x.VendorId == user.Id).Include(o => o.OfferInfo).ToList();
                double max = list.Max(x => x.OfferInfo.Price);
                return Ok(max);
            }
        }

    }
}
