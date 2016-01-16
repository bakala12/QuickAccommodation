using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AccommodationWebPage.Authorization
{
    public class AuthorizationRequiredAttribute : AuthorizeAttribute
    {
        public AuthorizationRequiredAttribute()
        {
            Order = 999;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (filterContext.HttpContext.User?.Identity == null ||
                filterContext.HttpContext.User?.Identity.IsAuthenticated == false)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Account", action = "AccessDenied" }));
            }
        }
    }
}