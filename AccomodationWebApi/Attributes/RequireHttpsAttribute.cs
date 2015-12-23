using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace AccomodationWebApi.Attributes
{
    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden)
                {
                    ReasonPhrase = "HTTPS required"
                };
            }
            else
            {
                base.OnAuthorization(actionContext);
            }
        }
    }
}