﻿using AccommodationDataAccess.Model;
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
        public class AddressesProxy : WebApiProxy
        {
            public AddressesProxy() : base("Addresses")
            {

            }

            public async Task<Address> Get(int id)
            {
                return await this.Get<Address>(id.ToString());
            }

            //public async Task<IList<Offer>> Search(string searchPhrase)
            //{
            //    return await this.Get<IList<Offer>>(string.Concat("Search/", HttpUtility.UrlEncode(searchPhrase)));
            //}
        }
    }