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
    [RoutePrefix("api/offerinfoes")]
    public class OfferInfoesController : ApiController
    {

        public IHttpActionResult Get(int id)
        {
            OfferInfo offerinfo = null;

            using (var context = new AccommodationContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                offerinfo = context.OfferInfo.FirstOrDefault(o => o.Id == id);
            }

            if (offerinfo == null)
            {
                return (IHttpActionResult)NotFound();
            }
            return Ok(offerinfo);

          
        }
    }
}
