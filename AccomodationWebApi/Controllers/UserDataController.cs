using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationShared.Dtos;
using AccomodationWebApi.Attributes;
using UserAuthorizationSystem.Registration;
using AccomodationWebApi.Providers;

namespace AccomodationWebApi.Controllers
{
    /// <summary>
    /// Provides a way to edit the user data. Due to security of user data all of this methods uses 
    /// save HTTPS protocol.
    /// </summary>
    [RoutePrefix("api/UserData")]
    public class UserDataController : ApiController
    {

        private readonly IContextProvider _provider;

        public UserDataController(IContextProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            _provider = provider;
        }

        public UserDataController()
        {
            _provider = new ContextProvider<AccommodationContext>();
        }


        /// <summary>
        /// Gets the data of the current user.
        /// </summary>
        /// <param name="username">The name of the user.</param>
        /// <returns>The data of the user.</returns>
        [Route("data/{username?}"), HttpGet]
        [RequireHttps]
        public IHttpActionResult GetUserData(string username)
        {
            User user = null;
            UserBasicDataDto returnDto = new UserBasicDataDto();
            using (var context = _provider.GetNewContext())
            {
                user = context.Users.First(u => u.Username.Equals(username));
                var data = context.UserData.First(ud => ud.Id == user.UserDataId);
                if (user == null) return NotFound();
                returnDto.FirstName = data.FirstName;
                returnDto.LastName = data.LastName;
                returnDto.CompanyName = data.CompanyName;
                returnDto.Email = data.Email;
            }
            return Ok(returnDto);
        }

        /// <summary>
        /// Changes the user data.
        /// </summary>
        /// <param name="dto">New data for the user</param>
        /// <returns>The status of the operation.</returns>
        [Route("changeData"), HttpPost]
        [RequireHttps]
        public IHttpActionResult ChangeUserData(ChangeUserDataDto dto)
        {
            using (var context = _provider.GetNewContext())
            {
                using (var transaction = new TransactionScope())
                {
                    User user = context.Users.FirstOrDefault(u => u.Username.Equals(dto.Username));
                    if (user == null) return NotFound();
                    UserData data = new UserData()
                    {
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        CompanyName = dto.CompanyName,
                        Email = dto.Email
                    };
                    user.UserData = data;
                    context.SaveChanges();
                    transaction.Complete();
                }
            }
            return Ok(true);
        }

        /// <summary>
        /// Changes the user password.
        /// </summary>
        /// <param name="dto">An object containing new user's credentials.</param>
        /// <returns>Operation complete status.</returns>
        [Route("changePassword"), HttpPost]
        [RequireHttps]
        public IHttpActionResult ChangeUserPassword(UserNewPasswordDto dto)
        {
            IRegisterUser register = new UserRegister();
            User newUser = register.GetNewUser(dto.Username, dto.NewPassword);
            using (var context = _provider.GetNewContext())
            {
                using (var transaction = new TransactionScope())
                {
                    User user = context.Users.FirstOrDefault(u => u.Username.Equals(dto.Username));
                    if (user == null) return NotFound();
                    user.Salt = newUser.Salt;
                    user.HashedPassword = newUser.HashedPassword;
                    context.SaveChanges();
                    transaction.Complete();
                }
            }
            return Ok(true);
        }

        /// <summary>
        /// Gets the rank of the user.
        /// </summary>
        /// <param name="username">The name of the user.</param>
        /// <returns>The rank of the given user.</returns>
        [Route("rank/{username?}"), HttpGet]
        public IHttpActionResult GetUserRank(string username)
        {
            using (var context = _provider.GetNewContext())
            {
                using (var transaction = new TransactionScope())
                {
                    User user = context.Users.FirstOrDefault(u => u.Username.Equals(username));
                    if (user == null) NotFound();
                    Rank rank = context.Ranks.FirstOrDefault(r => r.Id == user.Rank.Id);
                    transaction.Complete();
                    return Ok(rank?.Name);
                }
            }
        }
    }
}
