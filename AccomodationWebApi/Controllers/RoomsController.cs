﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccomodationWebApi.Providers;
using System.Data.Entity;
using AccomodationWebApi.Attributes;

namespace AccomodationWebApi.Controllers
{
    [RoutePrefix("api/Rooms")]
    public class RoomsController : ApiController
    {
        private readonly IContextProvider _provider;

        public RoomsController(IContextProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            _provider = provider;
        }

        public RoomsController()
        {
            _provider = new ContextProvider<AccommodationContext>();
        }

        /// <summary>
        /// Wysyła pokój o danym id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public IHttpActionResult Get(int id)
        {
            Room room = null;
            using (var context = _provider.GetNewContext())
            {
                if(context is DbContext)
                    (context as DbContext).Configuration.ProxyCreationEnabled = false;
                room = context.Rooms.FirstOrDefault(r => r.Id == id);
            }
            if (room == null) return NotFound();
            return Ok(room);
        }
    }
}
