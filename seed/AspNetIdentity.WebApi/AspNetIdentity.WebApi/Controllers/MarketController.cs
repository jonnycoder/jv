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
        private CustomJwtFormat jwtFormatter = new CustomJwtFormat("http://jv.com");

        [Route("resources")]
        [HttpGet]
        public async Task<IHttpActionResult> Resources()
        {
            var authHeader = Request.Headers.GetValues("Authorization");
            //if (null == authHeader || authHeader.Count() == 0)
            //{
            //    return StatusCode(System.Net.HttpStatusCode.Unauthorized);
            //}

            var jwtString = authHeader.ElementAt(0).Replace("Bearer ", "");
            if (string.IsNullOrEmpty(jwtString) || jwtString.Equals("null"))
            {
                return StatusCode(System.Net.HttpStatusCode.Unauthorized);
            }

            ClaimsPrincipal principal = await Task.Run(() => jwtFormatter.Validate(jwtString));
            if (null == principal)
            {
                return StatusCode(System.Net.HttpStatusCode.Unauthorized);
            }


            ModelFactory.UserResourceResponse response = new ModelFactory.UserResourceResponse();
            var roles = (from c in principal.Claims where c.Type.ToLower().Contains("role") select c.Value).ToList();

            if (roles.Contains("Affiliate"))
            {
                response.programs = GetPrograms();

            }
            if (roles.Contains("Vendor"))
            {
                response.affiliates = GetAffiliates(); 
            }

            return Json<ModelFactory.UserResourceResponse>(response);
        }

        public List<AffiliateReturnModel> GetAffiliates()
        {
            IEnumerable<AffiliateReturnModel> affiliates = MarketManager.GetAllAffiliates().ToList().Select( a => new AffiliateReturnModel { Email = a.Email, FirstName = a.FirstName, IndividualDescription = a.IndividualDescription, LastName = a.LastName, PhoneNumber = a.PhoneNumber, SkypeHandle = a.SkypeHandle, Username = a.UserName });
           
            return affiliates.ToList();
        }

        public List<ProgramReturnModel> GetPrograms()
        {
            IEnumerable<ProgramReturnModel> programs = MarketManager.Programs.ToList().Select(p => new ProgramReturnModel {  CreatedDate = p.CreatedDate.ToShortDateString(), ProgramDescription = p.Description, ProgramName = p.Name, ProgramUrl = p.Url });

            return programs.ToList();
        }
    }
}