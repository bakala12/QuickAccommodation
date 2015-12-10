using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccomodationWebApi.Attributes;
using UserAuthorizationSystem.Authentication;
using UserAuthorizationSystem.Identities;

namespace AccomodationWebApi.Controllers
{
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {
        [Route("user"), HttpPost]
        [RequireHttps]
        public IHttpActionResult GetUserIdentity(string[] credentials)
        {
            string username = credentials?[0];
            string password = credentials?[1];
            IUserAuthenticationService service = new UserAuthenticationService();
            var user=service.AuthenticateUser<AccommodationContext>(username, password);
            return Ok(user);
        }
    }
}
