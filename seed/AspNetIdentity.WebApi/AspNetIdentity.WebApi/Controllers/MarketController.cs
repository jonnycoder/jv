using AspNetIdentity.WebApi.Infrastructure;
using AspNetIdentity.WebApi.Providers;
using AspNetIdentity.WebApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace AspNetIdentity.WebApi.Controllers
{
    [RoutePrefix("api/market")]
    public class MarketController : BaseApiController
    {

        [Route("resources")]
        [HttpGet]
        [JwtRequired]
        public async Task<IHttpActionResult> Resources()
        {
            ClaimsPrincipal principal = this.User as ClaimsPrincipal;
            if (null == principal)
            {
                return StatusCode(System.Net.HttpStatusCode.Unauthorized);
            }
            var roles = (from c in principal.Claims where c.Type.ToLower().Contains("role") select c.Value).ToList();
      
            ModelFactory.UserResourceResponse response = await Task.Run(() =>
            {
                ModelFactory.UserResourceResponse innerResponse = new ModelFactory.UserResourceResponse();
                if (roles.Contains("Affiliate"))
                {
                    innerResponse.programs = MarketManager.GetPrograms();
                    innerResponse.unlockedPrograms = MarketManager.UnlockedPrograms(principal.Identities.First().GetUserId(), AppUserManager);
                }
                if (roles.Contains("Vendor"))
                {
                    innerResponse.affiliates = MarketManager.GetAffiliates();
                    innerResponse.unlockedAffiliates = MarketManager.UnlockedAffiliates(principal.Identities.First().GetUserId());
                }
                return innerResponse;
            });

            return Json<ModelFactory.UserResourceResponse>(response);
        }

    }
}