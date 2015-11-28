using AspNetIdentity.WebApi.Infrastructure;
using MySql.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;

namespace AspNetIdentity.WebApi.Models
{
    public class ModelFactory
    {

        private UrlHelper _UrlHelper;
        private ApplicationUserManager _AppUserManager;

        public ModelFactory(HttpRequestMessage request, ApplicationUserManager appUserManager)
        {
            _UrlHelper = new UrlHelper(request);
            _AppUserManager = appUserManager;
        }

        public class UserResourceResponse
        {
            public List<AffiliateReturnModel> affiliates = new List<AffiliateReturnModel>();
            public List<ProgramReturnModel> programs = new List<ProgramReturnModel>();
            public List<AffiliateReturnModel> unlockedAffiliates = new List<AffiliateReturnModel>();
            public List<ProgramReturnModel> unlockedPrograms = new List<ProgramReturnModel>();
        }

        public AffiliateReturnModel CreateAffiliate(ApplicationUser appUser, UserExtension extension)
        {
            return new AffiliateReturnModel
            {
                Email = appUser.Email,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                PhoneNumber = appUser.PhoneNumber,
                Username = appUser.UserName,
                IndividualDescription = extension.IndividualDescription,
                SkypeHandle = extension.SkypeHandle
            };
        }

        public UserReturnModel Create(ApplicationUser appUser)
        {
            return new UserReturnModel
            {
                Url = _UrlHelper.Link("GetUserById", new { id = appUser.Id }),
                Id = appUser.Id,
                UserName = appUser.UserName,
                FullName = string.Format("{0} {1}", appUser.FirstName, appUser.LastName),
                Email = appUser.Email,
                EmailConfirmed = appUser.EmailConfirmed,
                Level = appUser.Level,
                JoinDate = appUser.JoinDate,
                Roles = _AppUserManager.GetRolesAsync(appUser.Id).Result,
                Claims = _AppUserManager.GetClaimsAsync(appUser.Id).Result
            };

        }

        public ExtendedUserReturnModel Create(ApplicationUser appUser, UserExtension extension)
        {
            return new ExtendedUserReturnModel
            {
                Url = _UrlHelper.Link("GetUserById", new { id = appUser.Id }),
                Id = appUser.Id,
                UserName = appUser.UserName,
                FullName = string.Format("{0} {1}", extension.FirstName, extension.LastName),
                FirstName = extension.FirstName,
                LastName = extension.LastName,
                Email = appUser.Email,
                EmailConfirmed = appUser.EmailConfirmed,
                Level = appUser.Level,
                Credits = extension.Credits.ToString(),
                IndividualDescription = extension.IndividualDescription,
                PhoneNumber = extension.PhoneNumber,
                SkypeHandle = extension.SkypeHandle,
                JoinDate = appUser.JoinDate,
                Roles = _AppUserManager.GetRolesAsync(appUser.Id).Result,
                Claims = _AppUserManager.GetClaimsAsync(appUser.Id).Result
            };

        }

        public RoleReturnModel Create(IdentityRole appRole)
        {

            return new RoleReturnModel
            {
                Url = _UrlHelper.Link("GetRoleById", new { id = appRole.Id }),
                Id = appRole.Id,
                Name = appRole.Name
            };

        }
    }

    public class UserReturnModel
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public int Level { get; set; }
        public DateTime JoinDate { get; set; }
        public IList<string> Roles { get; set; }
        public IList<System.Security.Claims.Claim> Claims { get; set; }
    }

    public class ExtendedUserReturnModel
    {

        public string Url { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string SkypeHandle { get; set; }
        public string IndividualDescription { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Credits { get; set; }
        public bool EmailConfirmed { get; set; }
        public int Level { get; set; }
        public DateTime JoinDate { get; set; }
        public IList<string> Roles { get; set; }
        public IList<System.Security.Claims.Claim> Claims { get; set; }

    }

    public class AffiliateReturnModel
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SkypeHandle { get; set; }
        public string PhoneNumber { get; set; }
        public string IndividualDescription { get; set; }
        public string UserId { get; set; }
        public int MyRating { get; set; }
        public int AvgRating { get; set; }
        public string CategoryDescription { get; set; }
    }

    public class ProgramReturnModel
    {
        public string ProgramUrl { get; set; }
        public string ProgramName { get; set; }
        public string ProgramDescription { get; set; }
        public string CreatedDate { get; set; }
        public string CreatorSkypeHandle { get; set; }
        public string CreatorFullName { get; set; }
        public string CreatorPhoneNumber { get; set; }
    }



    public class RoleReturnModel
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}