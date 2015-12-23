using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccomodationWebApi.Attributes;
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

        
    [RoutePrefix("api/addresses")]
    public class AddressesController : ApiController
    {
        /// <summary>
        /// Dostawca kontekstu bazy danych
        /// </summary>
        private readonly IContextProvider _provider;
        
        /// <summary>
        /// Konstruktor przyjmujący dostawcę kontekstu bazy danych
        /// </summary>
        /// <param name="provider"></param>
        public AddressesController(IContextProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            _provider = provider;
        }

        /// <summary>
        /// Konstruktor bezparametrowy (dostawca AccommodationContext )
        /// </summary>
        public AddressesController()
        {
            _provider = new ContextProvider<AccommodationContext>();
        }

        /// <summary>
        /// wysyła adres o danym id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
