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
    [RoutePrefix("api/places")]
    public class PlacesController : ApiController
    {

        public IHttpActionResult Get(int id)
        {
            Place place = null;

            using (var context = new AccommodationContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                place = context.Places.FirstOrDefault(o => o.Id == id);
            }

            if (place == null)
            {
                return (IHttpActionResult)NotFound();
            }
            return Ok(place);

          
        }
    }
}
