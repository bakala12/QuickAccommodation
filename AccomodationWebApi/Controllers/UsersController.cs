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
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {

        //public IHttpActionResult Get(int id)
        //{
        //    User user = null;

        //    using (var context = new AccommodationContext())
        //    {
        //        context.Configuration.ProxyCreationEnabled = false;
        //        user = context.Users.FirstOrDefault(o => o.Id.Equals(id));
        //    }

        //    if (user == null)
        //    {
        //        return (IHttpActionResult)NotFound();
        //    }
        //    return Ok(user);
        //}

        [Route("getuser/{username?}"), HttpGet]
        public IHttpActionResult Get(string username = null)
        {
            username = username ?? string.Empty;
            User user = null;

            using (var context = new AccommodationContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
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
