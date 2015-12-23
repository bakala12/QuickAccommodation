using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

namespace AccommodationApplication.Services
{
    /// <summary>
    /// Proxy dla pokoi
    /// </summary>
    public class RoomsProxy : WebApiProxy
    {

        public RoomsProxy(): base("Rooms", false)
        {
        }

        /// <summary>
        /// Wysyła zapytanie o pokój o danym id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Room> Get(int id)
        {
            return await Get<Room>(id.ToString());
        }
    }
}
