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
        public class OfferInfoesProxy : WebApiProxy
        {
            public OfferInfoesProxy() : base("OfferInfoes")
            {

            }

            public async Task<OfferInfo> Get(int id)
            {
                return await this.Get<OfferInfo>(id.ToString());
            }

            //public async Task<IList<Offer>> Search(string searchPhrase)
            //{
            //    return await this.Get<IList<Offer>>(string.Concat("Search/", HttpUtility.UrlEncode(searchPhrase)));
            //}
        }
    }
