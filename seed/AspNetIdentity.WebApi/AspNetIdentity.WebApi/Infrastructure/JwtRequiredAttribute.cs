using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using AspNetIdentity.WebApi.Providers;
using System.Threading;

namespace AspNetIdentity.WebApi.Infrastructure
{

    public class JwtRequiredAttribute : ActionFilterAttribute
    {
        private CustomJwtFormat jwtFormatter = new CustomJwtFormat("http://jv.com");

        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            IEnumerable<string> authHeaderValues = null;
            var authHeader = actionContext.Request.Headers.TryGetValues("Authorization", out authHeaderValues);

            if (null == authHeaderValues || authHeaderValues.Count() == 0)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Task.FromResult<object>(null);
            }
            var jwtString = authHeaderValues.ElementAt(0).Replace("Bearer ", "");
            if (string.IsNullOrEmpty(jwtString) || jwtString.Equals("null"))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Task.FromResult<object>(null);
            }
            ClaimsPrincipal principal = null;
            try
            {
                principal =  jwtFormatter.Validate(jwtString);
            }
            catch { }
            if (null == principal)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Task.FromResult<object>(null);
            }

            if (!principal.Identity.IsAuthenticated)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Task.FromResult<object>(null);
            }

            var controller = (actionContext.ControllerContext.Controller as ApiController);
            controller.User = principal;

            //User is Authorized, complete execution
            return Task.FromResult<object>(null);
        }
    }
}