using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UserAuthorizationSystem.Identities;
using AccomodationWebApi.Attributes;

namespace AccommodationApplication.Services
{
    public class LoginProxy : WebApiProxy
    {
        public LoginProxy() : base("Login", true)
        {
        }

        public async Task<CustomIdentity> GetUserAsync(string username, string clearTextPassword)
        {
            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            return await Get<CustomIdentity>("user/"+HttpUtility.UrlEncode(username)+"/"+HttpUtility.UrlEncode(clearTextPassword));
        }
    }
}
