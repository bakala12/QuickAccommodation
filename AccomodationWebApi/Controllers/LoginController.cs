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
using AccomodationWebApi.Providers;

namespace AccomodationWebApi.Controllers
{
    /// <summary>
    /// Provides a way to authenticate user and register user in the application.
    /// All the methods in this class must be save, so they uses save HTTPS protocol.
    /// </summary>
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {

        private readonly IContextProvider _provider;

        public LoginController(IContextProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            _provider = provider;
        }

        public LoginController()
        {
            _provider = new ContextProvider<AccommodationContext>();
        }


        /// <summary>
        /// Gets the authenticated user identity.
        /// </summary>
        /// <param name="dto">Object containing user's credential data.</param>
        /// <returns>The identity of the user.</returns>
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

        /// <summary>
        /// Sign up a new user into the application.
        /// </summary>
        /// <param name="dto">All kind of user data.</param>
        /// <returns>Operation complete status.</returns>
        [Route("save"), HttpPost]
        [RequireHttps]
        public IHttpActionResult SaveUserAsync(UserAllDataDto dto)
        {
            IRegisterUser regster = new UserRegister();
            regster.SaveUser<AccommodationContext>(dto.User, dto.UserData, dto.Address);
            return Ok();
        }

        /// <summary>
        /// Gets an object representing a new user.
        /// </summary>
        /// <param name="dto">Some initializing user credentials.</param>
        /// <returns>An object representing a user.</returns>
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
