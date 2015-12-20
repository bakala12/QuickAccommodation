using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;

namespace AccomodationWebApi.Controllers
{
    [RoutePrefix("api/Rooms")]
    public class RoomsController : ApiController
    {
        public IHttpActionResult Get(int id)
        {
            Room room = null;
            using (var context = new AccommodationContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                room = context.Rooms.FirstOrDefault(r => r.Id == id);
            }
            if (room == null) return NotFound();
            return Ok(room);
        }
    }
}
