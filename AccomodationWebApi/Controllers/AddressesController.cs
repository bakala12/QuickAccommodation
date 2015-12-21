using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccomodationWebApi.Providers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        private readonly IContextProvider _provider;

        public AddressesController(IContextProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            _provider = provider;
        }

        public AddressesController()
        {
            _provider = new ContextProvider<AccommodationContext>();
        }


        public IHttpActionResult Get(int id)
        {
            Address address = null;

            using (var context = _provider.GetNewContext())
            {
                if (context is DbContext) (context as DbContext).Configuration.ProxyCreationEnabled = false;
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
