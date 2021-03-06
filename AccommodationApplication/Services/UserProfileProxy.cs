﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AccommodationDataAccess.Model;
using AccommodationShared.Dtos;

namespace AccommodationApplication.Services
{
    public class UserProfileProxy : WebApiProxy
    {
        public UserProfileProxy() : base("UserData", true)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        public async Task<UserBasicDataDto> GetUserAsync(string username)
        {
            if (string.IsNullOrEmpty(username)) return null;
            return await Get<UserBasicDataDto>("data/" + HttpUtility.UrlEncode(username));
        }

        public async Task ChangeUserDataAsync(string username, UserBasicDataDto dto)
        {
            ChangeUserDataDto dataDto = new ChangeUserDataDto()
            {
                Username = username,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                CompanyName = dto.CompanyName,
                Email = dto.Email
            };
            await Post<ChangeUserDataDto, bool>("changeData", dataDto);
        }

        public async Task ChangeUserPasswordAsync(UserNewPasswordDto dto)
        {
            await Post<UserNewPasswordDto, bool>("changePassword", dto);
        }

        public async Task<string> GetUserRankAsync(string username)
        {
            return await Get<string>("rank/" + HttpUtility.UrlEncode(username));
        }
    }
}
