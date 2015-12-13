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
    }
}
