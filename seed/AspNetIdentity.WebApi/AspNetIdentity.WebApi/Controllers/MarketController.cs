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
                    innerResponse.programs = GetPrograms();
                }
                if (roles.Contains("Vendor"))
                {
                    innerResponse.affiliates = GetAffiliates();
                }
                return innerResponse;
            });

            return Json<ModelFactory.UserResourceResponse>(response);
        }

        public List<AffiliateReturnModel> GetAffiliates()
        {
            IEnumerable<AffiliateReturnModel> affiliates = MarketManager.GetAllAffiliates().ToList().Select(a => new AffiliateReturnModel { Email = a.Email, FirstName = a.FirstName, IndividualDescription = a.IndividualDescription, LastName = a.LastName, PhoneNumber = a.PhoneNumber, SkypeHandle = a.SkypeHandle, Username = a.UserName });

            return affiliates.ToList();
        }

        public List<ProgramReturnModel> GetPrograms()
        {
            IEnumerable<ProgramReturnModel> programs = MarketManager.Programs.ToList().Select(p => new ProgramReturnModel { CreatedDate = p.CreatedDate.ToShortDateString(), ProgramDescription = p.Description, ProgramName = p.Name, ProgramUrl = p.Url });

            return programs.ToList();
        }
    }
}