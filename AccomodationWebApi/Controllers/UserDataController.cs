using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationShared.Dtos;
using AccomodationWebApi.Attributes;
using UserAuthorizationSystem.Registration;

namespace AccomodationWebApi.Controllers
{
    [RoutePrefix("api/UserData")]
    public class UserDataController : ApiController
    {
        [Route("data"), HttpPost]
        [RequireHttps]
        public IHttpActionResult GetUserData(UserCredentialDto dto)
        {
            string username = dto.Username;
            User user = null;
            UserBasicDataDto returnDto = new UserBasicDataDto();
            using (var context = new AccommodationContext())
            {
                user = context.Users.First(u => u.Username.Equals(username));
                var data = context.UserData.First(ud => ud.Id == user.UserDataId);
                if (user == null) return null;
                returnDto.FirstName = data.FirstName;
                returnDto.LastName = data.LastName;
                returnDto.CompanyName = data.CompanyName;
                returnDto.Email = data.Email;
            }
            return Ok(returnDto);
        }

        [Route("changeData"), HttpPost]
        [RequireHttps]
        public IHttpActionResult ChangeUserData(ChangeUserDataDto dto)
        {
            using (var context = new AccommodationContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    User user = context.Users.FirstOrDefault(u=>u.Username.Equals(dto.Username));
                    if (user == null) return null;
                    UserData data = new UserData()
                    {
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        CompanyName = dto.CompanyName,
                        Email = dto.Email
                    };
                    user.UserData = data;
                    context.SaveChanges();
                    transaction.Commit();
                }
            }
            return Ok(true);
        }

        [Route("changePassword"), HttpPost]
        [RequireHttps]
        public IHttpActionResult ChangeUserPassword(UserNewPasswordDto dto)
        {
            IRegisterUser register = new UserRegister();
            User newUser = register.GetNewUser(dto.Username, dto.NewPassword);
            using (var context = new AccommodationContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    User user = context.Users.FirstOrDefault(u => u.Username.Equals(dto.Username));
                    if (user == null) return null;
                    user.Salt = newUser.Salt;
                    user.HashedPassword = newUser.HashedPassword;
                    context.SaveChanges();
                    transaction.Commit();
                }
            }
            return Ok(true);
        }
    }
}
