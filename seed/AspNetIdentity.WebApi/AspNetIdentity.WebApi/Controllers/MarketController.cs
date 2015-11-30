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
                    innerResponse.unlockedPrograms = MarketManager.UnlockedPrograms(principal.Identities.First().GetUserId());
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


        [Route("revealuser")]
        [HttpPost]
        [JwtRequired]
        public async Task<IHttpActionResult> RevealUser(RevealUserBindingModel revealRequest)
        {
            ClaimsPrincipal principal = this.User as ClaimsPrincipal;
            if (null == principal)
            {
                return StatusCode(System.Net.HttpStatusCode.Unauthorized);
            }

           string caller = principal.Identities.First().GetUserId();

            bool response = await Task.Run(() =>
            {
                return MarketManager.RevealUserFor(caller, revealRequest.UserId);   
            });

            if (response)
            {
                return StatusCode(System.Net.HttpStatusCode.Created);
            }
            else
            {
                return StatusCode(System.Net.HttpStatusCode.InternalServerError);
            }

        }

        [Route("revealprogram")]
        [HttpPost]
        [JwtRequired]
        public async Task<IHttpActionResult> RevealProgram(RevealProgramBindingModel revealRequest)
        {
            ClaimsPrincipal principal = this.User as ClaimsPrincipal;
            if (null == principal)
            {
                return StatusCode(System.Net.HttpStatusCode.Unauthorized);
            }

            string caller = principal.Identities.First().GetUserId();

            bool response = await Task.Run(() =>
            {
                return MarketManager.RevealProgramFor(caller, revealRequest.ProgramName);
            });

            if (response)
            {
                return StatusCode(System.Net.HttpStatusCode.Created);
            }
            else
            {
                return StatusCode(System.Net.HttpStatusCode.InternalServerError);
            }

        }

    }
}