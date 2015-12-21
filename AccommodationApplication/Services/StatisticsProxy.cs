using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AccommodationApplication.Services
{
    public class StatisticsProxy : WebApiProxy
    {
        public StatisticsProxy() : base("Statistics", false)
        {
            
        }

        public async Task<string> GetUserRank(string username)
        {
            return await Get<string>("rank/" + HttpUtility.UrlEncode(username));
        }

        public async Task<int> GetUserOffersCount(string username)
        {
            return await Get<int>("offersCount/" + HttpUtility.UrlEncode(username));
        }

        public async Task<int> GetReservedOffersCount(string username)
        {
            return await Get<int>("reservedOffersCount/" + HttpUtility.UrlEncode(username));
        }
    }
}
