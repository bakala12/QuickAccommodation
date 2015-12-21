using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Channels;
using System.Web.Http;
using AccommodationDataAccess.Domain;
using AccomodationWebApi.Providers;

namespace AccomodationWebApi.Controllers
{
    [RoutePrefix("api/Statistics")]
    public class StatisticsController : ApiController
    {
        private readonly IContextProvider _provider;

        public StatisticsController(IContextProvider provider)
        {
            if(provider==null) throw new ArgumentNullException(nameof(provider));
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
                string name=user.Rank.Name;
                return Ok(name);
            }
        }
    }
}
