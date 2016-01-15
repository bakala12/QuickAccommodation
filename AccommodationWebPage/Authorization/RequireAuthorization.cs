using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AccommodationWebPage.Authorization
{
    public class RequireAuthorization : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return Authorization.Current.IsAuthenticated;
        }
    }
}
