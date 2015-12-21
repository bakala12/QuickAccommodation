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
    /// Proxy dla informacji o ofercie
    /// </summary>
    public class OfferInfoesProxy : WebApiProxy
    {
        public OfferInfoesProxy() : base("OfferInfoes")
        {
        }

        /// <summary>
        /// Pobiera informację o ofercie o danym id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OfferInfo> Get(int id)
        {
            return await this.Get<OfferInfo>(id.ToString());
        }
    }
}
