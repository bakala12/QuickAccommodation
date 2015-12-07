using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
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
        public IEnumerable<Offer> GetAllOffers()
        {
            List<Offer> ret = new List<Offer>();

            using (var context = new AccommodationContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                ret = context.Offers.Take(20).ToList();
                context.SaveChanges();
            }

            return ret;

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
    }
}
