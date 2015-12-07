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
    [RoutePrefix("api/addresses")]
    public class AddressesController : ApiController
    {

        public IHttpActionResult Get(int id)
        {
            Address address = null;

            using (var context = new AccommodationContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                address = context.Addresses.FirstOrDefault(o => o.Id == id);
            }

            if (address == null)
            {
                return (IHttpActionResult)NotFound();
            }
            return Ok(address);


        }
    }
}
