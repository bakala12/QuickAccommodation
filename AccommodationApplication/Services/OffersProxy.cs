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
    public class OffersProxy : WebApiProxy
    {
        public OffersProxy() : base("Offers")
        {

        }

        public async Task<Offer> Get(int id)
        {
            return await this.Get<Offer>(id.ToString());
        }

        public async Task<IList<Offer>> GetUserOffers(int userId)
        {
            return await this.Get<IList<Offer>>(string.Concat("GetUserOffers/", HttpUtility.UrlEncode(userId.ToString())));
        }
    }
}
