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
    /// Proxy dla adresów
    /// </summary>
    public class AddressesProxy : WebApiProxy
    {
        public AddressesProxy() : base("Addresses")
        {
        }

        /// <summary>
        /// Wysyła zapytanie o adres o danym id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Address> Get(int id)
        {
            return await this.Get<Address>(id.ToString());
        }

    }
}
