using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AccommodationDataAccess.Model;
using AccommodationShared.Dtos;
using UserAuthorizationSystem.Identities;
using AccomodationWebApi.Attributes;

namespace AccommodationApplication.Services
{
    public class LoginProxy : WebApiProxy
    {
        public LoginProxy() : base("Login", true)
        {
            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
        }

        public async Task<CustomIdentity> GetUserAsync(string username, string clearTextPassword)
        {
            UserCredentialDto dto = new UserCredentialDto()
            {
                Username = username,
                Password = clearTextPassword
            };
            return await Post<UserCredentialDto, CustomIdentity>("user",dto);
        }

        public async Task SaveUserAsync(User user, UserData data, Address address)
        {
            UserAllDataDto dto = new UserAllDataDto()
            {
                User = user,
                UserData = data,
                Address = address
            };
            await Post<UserAllDataDto, object>("save", dto);
        }

        public async Task<User> GetNewUserAsync(string username, string clearTextPassword)
        {
            UserCredentialDto dto = new UserCredentialDto()
            {
                Username = username,
                Password = clearTextPassword
            };
            return await Post<UserCredentialDto, User>("newUser", dto);
        }
    }
}
