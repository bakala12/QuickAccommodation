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
    public class PlacesProxy : WebApiProxy
    {
        public PlacesProxy() : base("Places")
        {

        }

        public async Task<Place> Get(int id)
        {
            return await this.Get<Place>(id.ToString());
        }
    }
}
