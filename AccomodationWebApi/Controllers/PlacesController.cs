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

    [RoutePrefix("api/places")]
    public class PlacesController : ApiController
    {

        private readonly IContextProvider _provider;

        public PlacesController(IContextProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            _provider = provider;
        }

        public PlacesController()
        {
            _provider = new ContextProvider<AccommodationContext>();
        }

        /// <summary>
        /// Wysyła miejsce o danym id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public IHttpActionResult Get(int id)
        {
            Place place = null;

            using (var context = _provider.GetNewContext())
            {
                (context as DbContext).Configuration.ProxyCreationEnabled = false;
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
