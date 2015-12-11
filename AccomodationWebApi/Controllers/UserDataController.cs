﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AccommodationDataAccess.Domain;
using AccommodationDataAccess.Model;
using AccommodationShared.Dtos;
using AccomodationWebApi.Attributes;

namespace AccomodationWebApi.Controllers
{
    [RoutePrefix("api/UserData")]
    public class UserDataController : ApiController
    {
        [Route("data"), HttpPost]
        [RequireHttps]
        public IHttpActionResult GetUserData(UserCredentialDto dto)
        {
            string username = dto.Username;
            User user = null;
            UserBasicDataDto returnDto = new UserBasicDataDto();
            using (var context = new AccommodationContext())
            {
                user = context.Users.First(u => u.Username.Equals(username));
                var data = context.UserData.First(ud => ud.Id == user.UserDataId);
                if (user == null) return null;
                returnDto.FirstName = data.FirstName;
                returnDto.LastName = data.LastName;
                returnDto.CompanyName = data.CompanyName;
                returnDto.Email = data.Email;
            }
            return Ok(returnDto);
        }
    }
}