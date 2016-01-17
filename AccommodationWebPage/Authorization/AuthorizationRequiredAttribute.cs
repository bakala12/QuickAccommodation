using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AccommodationWebPage.Authorization
{
    /// <summary>
    /// Atrybut zabezpieczający przed dostępem użytkownika niezalogowanego.
    /// </summary>
    public class AuthorizationRequiredAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Inicjalizuje nową instancję atrybutu AuthorizationRequired.
        /// </summary>
        public AuthorizationRequiredAttribute()
        {
            Order = 999;
        }

        /// <summary>
        /// Nadpisuje metodę OnAuthorization do autoryzacji.
        /// </summary>
        /// <param name="filterContext">Kontekst autoryzacji.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (string.IsNullOrEmpty(filterContext.HttpContext.User?.Identity?.Name) 
                || filterContext.HttpContext.User?.Identity.IsAuthenticated == false)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Account", action = "AccessDenied" }));
            }
        }
    }
}