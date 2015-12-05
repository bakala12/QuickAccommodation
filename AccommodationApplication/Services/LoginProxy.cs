using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UserAuthorizationSystem.Identities;

namespace AccommodationApplication.Services
{
    public class LoginProxy : WebApiProxy
    {
        public LoginProxy() : base("Login")
        {
        }

        public async Task<CustomIdentity> GetUserAsync(string username, string clearTextPassword)
        {
            return await Get<CustomIdentity>("user/" + HttpUtility.UrlEncode(username)+"/"+HttpUtility.UrlEncode(clearTextPassword));
        }
    }
}
