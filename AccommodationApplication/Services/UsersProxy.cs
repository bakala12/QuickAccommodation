using AccommodationDataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AccommodationApplication.Services
{
    public class UsersProxy : WebApiProxy
    {
        public UsersProxy() : base("Users")
        {

        }

        //public async Task<User> GetUser(int username)
        //{
        //    return await this.Get<User>(username.ToString());
        //}

        public async Task<User> GetUser(string username)
        {
            return await this.Get<User>(string.Concat("GetUser/", HttpUtility.UrlEncode(username)));
        }

    }
}
