using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccommodationWebPage.Authorization
{
    public class AuthenticationFilterAttribute : FilterAttribute, IAuthorizationFilter
    {
        private static readonly Authenticator Authenticator = Authenticator.Instance;

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var cookie = filterContext.HttpContext.Request.Cookies["auth"];
            if (cookie != null)
            {
                var user = Authenticator.Authenticate(cookie.Value);
                var newCookie = new HttpCookie("auth");
                if (user != null)
                {
                    filterContext.HttpContext.User = user;

                    // Refresh cookie
                    newCookie.Value = cookie.Value;
                    newCookie.Expires = DateTime.Now.AddDays(1);
                }
                else
                {
                    // Invalidate cookie
                    newCookie.Value = string.Empty;
                    newCookie.Expires = DateTime.Now.AddDays(-1);
                }
                filterContext.HttpContext.Response.SetCookie(newCookie);
            }
        }
    }
}