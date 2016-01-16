using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccommodationWebPage.Authorization
{
    public class AuthorizationRequiredAttribute : AuthorizeAttribute
    {
        public AuthorizationRequiredAttribute()
        {
            Order = 999;
        }
    }
}