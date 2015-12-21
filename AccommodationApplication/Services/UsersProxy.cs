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
    /// <summary>
    /// Proxy dla userów
    /// </summary>
    public class UsersProxy : WebApiProxy
    {
        public UsersProxy() : base("Users")
        {
        }

        /// <summary>
        /// Wysysła zapytanie o usera o danej nazwie
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<User> GetUser(string username)
        {
            return await this.Get<User>(string.Concat("GetUser/", HttpUtility.UrlEncode(username)));
        }

        

    }
}
