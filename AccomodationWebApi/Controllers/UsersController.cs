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
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly IContextProvider _provider;

        public UsersController(IContextProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            _provider = provider;
        }

        public UsersController()
        {
            _provider = new ContextProvider<AccommodationContext>();
        }


        [Route("getuser/{username?}"), HttpGet]
        public IHttpActionResult Get(string username = null)
        {
            username = username ?? string.Empty;
            User user = null;

            using (var context = _provider.GetNewContext())
            {
                if (context is DbContext)
                    (context as DbContext).Configuration.ProxyCreationEnabled = false;
                user = context.Users.FirstOrDefault(o => o.Username.Equals(username));
            }

            if (user == null)
            {
                return (IHttpActionResult)NotFound();
            }
            return Ok(user);
        }
    }
}
