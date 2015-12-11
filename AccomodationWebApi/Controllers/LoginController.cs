using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationShared.Dtos;
using AccomodationWebApi.Attributes;
using UserAuthorizationSystem.Authentication;
using UserAuthorizationSystem.Identities;
using UserAuthorizationSystem.Registration;

namespace AccomodationWebApi.Controllers
{
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {
        [Route("user"), HttpPost]
        [RequireHttps]
        public IHttpActionResult GetUserIdentity(UserCredentialDto dto)
        {
            string username = dto.Username;
            string password = dto.Password;
            IUserAuthenticationService service = new UserAuthenticationService();
            var user=service.AuthenticateUser<AccommodationContext>(username, password);
            return Ok(user);
        }

        [Route("save"), HttpPost]
        [RequireHttps]
        public IHttpActionResult SaveUserAsync(UserAllDataDto dto)
        {
            IRegisterUser regster = new UserRegister();
            regster.SaveUser<AccommodationContext>(dto.User, dto.UserData, dto.Address);
            return Ok();
        }

        [Route("newUser"), HttpPost]
        [RequireHttps]
        public IHttpActionResult GetNewUserAsync(UserCredentialDto dto)
        {
            IRegisterUser register = new UserRegister();
            User user = register.GetNewUser(dto.Username, dto.Password);
            return Ok(user);
        }
    }
}
