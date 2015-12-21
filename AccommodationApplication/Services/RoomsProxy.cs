using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

namespace AccommodationApplication.Services
{
    public class RoomsProxy : WebApiProxy
    {
        public RoomsProxy(): base("Rooms", false)
        {
        }

        public async Task<Room> Get(int id)
        {
            return await Get<Room>(id.ToString());
        }
    }
}
