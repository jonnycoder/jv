using AspNetIdentity.WebApi.Infrastructure;
using AspNetIdentity.WebApi.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using AspNetIdentity.WebApi.Models;
using System.Security.Claims;

namespace AspNetIdentity.WebApi.Controllers
{
    class CustomerResourceResponse
    {
        public List<BusinessAffiliate> affiliates = new List<BusinessAffiliate>();
        public List<BusinessProgram> programs = new List<BusinessProgram>();
    }

    class BusinessProgram {
        public BusinessProgram(string _name)
        {
            this.name = _name;
        }
        public string name { get; set; }
    }
    class BusinessAffiliate {
        public BusinessAffiliate(string _name)
        {
            this.name = _name;
        }
        public string name { get; set; }
    }

    [RoutePrefix("api/market")]
    public class MarketController : BaseApiController
    {

        private CustomJwtFormat jwtFormatter = new CustomJwtFormat("http://jv.com");

        [Route("resources")]
        [HttpGet]
        public async Task<IHttpActionResult> Resources()
        {
            var authHeader = Request.Headers.GetValues("Authorization");
            if (null == authHeader || authHeader.Count() == 0)
            {
                return StatusCode(System.Net.HttpStatusCode.Unauthorized);
            }

            var jwtString = authHeader.ElementAt(0).Replace("Bearer ", "");
  
            IEnumerable<Claim> claims = await Task.Run( () => jwtFormatter.Validate(jwtString));

            CustomerResourceResponse response = new CustomerResourceResponse();
            var roles = (from c in claims where c.Type.ToLower().Contains("role") select c.Value).ToList();

            if (roles.Contains("Affiliate"))
            {
                response.affiliates.Add(new BusinessAffiliate("Bob Affiliate"));
                response.affiliates.Add(new BusinessAffiliate("Alex Affiliate"));
                response.affiliates.Add(new BusinessAffiliate("William Affiliate"));
            }
            if (roles.Contains("Vendor"))
            {
                response.programs.Add(new BusinessProgram("3 Marketeers LLC"));
                response.programs.Add(new BusinessProgram("4 Of Spades LLC"));
            }

            return Json<CustomerResourceResponse>(response);
        }
    }
}