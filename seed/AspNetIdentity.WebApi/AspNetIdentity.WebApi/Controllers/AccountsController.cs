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
using System.Web.Http.ModelBinding;
using System.Security.Claims;

namespace AspNetIdentity.WebApi.Controllers
{
    class LoginResponse
    {
        public int httpResult { get; set; }
        public string token { get; set; }
        public ExtendedUserReturnModel user { get; set; }
    }

    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IHttpActionResult> Login(LoginUserBindingModel loginUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApplicationUser lookup = AppUserManager.Find(loginUserModel.UserName, loginUserModel.Password);

            var loginResult = new LoginResponse();
            if (lookup != null && lookup.EmailConfirmed)
            {
                loginResult.token = await  GenerateUserToken(lookup);
                loginResult.user = TheModelFactory.Create( lookup, UserExtensionManager.UserExtensions.Where(e => e.UserId == lookup.Id).FirstOrDefault());
                loginResult.httpResult = Convert.ToInt16(System.Net.HttpStatusCode.OK);
                return Json<LoginResponse>(loginResult);
            }
            else
            {
                return StatusCode(System.Net.HttpStatusCode.Unauthorized);
            }
        }

        private async Task<string> GenerateUserToken(ApplicationUser validatedUser)
        {
            CustomJwtFormat jwt = new CustomJwtFormat("http://jv.com");
            var identity = await validatedUser.GenerateUserIdentityAsync(AppUserManager, "password");
            Microsoft.Owin.Security.AuthenticationProperties properties = new Microsoft.Owin.Security.AuthenticationProperties();
            properties.IssuedUtc = DateTime.Now.ToUniversalTime();
            properties.ExpiresUtc = DateTime.Now.AddMinutes(5).ToUniversalTime();
            return jwt.Protect(new Microsoft.Owin.Security.AuthenticationTicket(identity, properties));
        }

        [AllowAnonymous]
        [Route("create")]
        public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel createUserModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool IsMarketer = false;
            if (null != createUserModel.Marketer && !bool.TryParse(createUserModel.Marketer, out IsMarketer))
            {
                return BadRequest();
            }

            bool IsAffiliate = false;
            if (null != createUserModel.Affiliate && !bool.TryParse(createUserModel.Affiliate, out IsAffiliate))
            {
                return BadRequest();
            }

            if (!IsMarketer && !IsAffiliate)
            {
                return BadRequest("Affiliate or Marketer must be selected");
            }

            // check if the user created a program
            if (IsMarketer)
            {
                string error = ValidateCreateModel(createUserModel, Role.Vendor);
                if (!String.IsNullOrEmpty(error))
                {
                    return BadRequest(ModelState);
                }
            }

            var user = new ApplicationUser()
            {
                UserName = createUserModel.Username,
                Email = createUserModel.Email,
                FirstName = createUserModel.FirstName,
                LastName = createUserModel.LastName,
                Level = 3,
                JoinDate = DateTime.Now.Date
            };

            IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, createUserModel.Password);

            if (!addUserResult.Succeeded)
            {
                return GetErrorResult(addUserResult);
            }

            // extend the user with specific information
            var userExt = new UserExtension()
            {
                SkypeHandle = createUserModel.SkypeHandle,
                UserId = user.Id,
                IndividualDescription = createUserModel.IndividualDescription,
                FirstName = createUserModel.FirstName,
                LastName = createUserModel.LastName,
                PhoneNumber = createUserModel.PhoneNumber
            };

            UserExtensionManager.UserExtensions.Add(userExt);

            try
            {
                int resultCount = await UserExtensionManager.Update();
            }
            catch (Exception ex)
            {
                // todo delete user here
                try
                {
                    await AppUserManager.DeleteAsync(user);
                }
                catch
                {
                    // do our best to not create secondary errors
                }

                return InternalServerError();
            }

            // check if the user created a program
            if (IsMarketer)
            {
                AppUserManager.AddToRole(user.Id, "Vendor");

                Program newProgram = new Program()
                {
                    CreatedDate = DateTime.Now,
                    CreatorId = user.Id,
                    Description = createUserModel.ProgramDescription,
                    Url = createUserModel.ProgramUrl,
                    Name = createUserModel.ProgramName
                };

                MarketManager.Programs.Add(newProgram);
                await MarketManager.Update();
            }

            if (IsAffiliate)
            {
                AppUserManager.AddToRole(user.Id, "Affiliate");
            }

            Uri locationHeader = await SendConfirm(user);

            return Created(locationHeader, TheModelFactory.Create(user, userExt));
        }

        private string ValidateCreateModel(CreateUserBindingModel createModel, Role role)
        {
            string msg = String.Empty;
            if (role.Equals(Role.Vendor))
            {
                if (String.IsNullOrEmpty(createModel.ProgramDescription))
                {
                    msg = "Please enter a Program Description";
                    ModelState.AddModelError("createUserModel.ProgramDescription", msg);
                }

                if (String.IsNullOrEmpty(createModel.ProgramName))
                {
                    msg = "Please enter a Program Name";
                    ModelState.AddModelError("createUserModel.ProgramName", msg);
                }

                Program program = MarketManager.GetAllPrograms().Where(p => p.Name.ToLower() == createModel.ProgramName.ToLower()).FirstOrDefault();
                if (null != program)
                {
                    msg = "That program name is already taken, please enter something different";
                    ModelState.AddModelError("createUserModel.ProgramName", msg);
                }


                if (String.IsNullOrEmpty(createModel.ProgramUrl))
                {
                    msg = "Please enter a Program Url";
                    ModelState.AddModelError("createUserModel.ProgramUrl", msg);
                }

            }

            return msg;
        }

        private async Task<Uri> SendConfirm(ApplicationUser user)
        {
            string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);

            var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));

            await this.AppUserManager.SendEmailAsync(user.Id,
                                                    "Confirm your account",
                                                    "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

            return locationHeader;
        }

        [Route("rate")]
        [HttpPost]
        [JwtRequired]
        public async Task<IHttpActionResult> Rate(RateUserBindingModel rating)
        {
            ClaimsPrincipal principal = this.User as ClaimsPrincipal;
            if (null == principal)
            {
                return StatusCode(System.Net.HttpStatusCode.Unauthorized);
            }

            //UserRatingManager.RegisterRating(
            //    principal.Identities.First().GetUserId(),
            //    rating.AffiliateId,
            //    rating.Rating);
         
            return StatusCode(System.Net.HttpStatusCode.Created);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("Err", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.AppUserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return GetErrorResult(result);
            }
        }


        [Authorize(Roles = "Admin")]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            var identity = User.Identity as System.Security.Claims.ClaimsIdentity;

            return Ok(this.AppUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            var user = await this.AppUserManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        [Authorize(Roles = "Admin")]
        [Route("user/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            var user = await this.AppUserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        [Authorize]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.AppUserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {

            //Only SuperAdmin or Admin can delete users (Later when implement roles)

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser != null)
            {
                IdentityResult result = await this.AppUserManager.DeleteAsync(appUser);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();

            }

            return NotFound();

        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await this.AppUserManager.GetRolesAsync(appUser.Id);

            var rolesNotExists = rolesToAssign.Except(this.AppRoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Count() > 0)
            {

                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            IdentityResult removeResult = await this.AppUserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await this.AppUserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();

        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/assignclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignClaimsToUser([FromUri] string id, [FromBody] List<ClaimBindingModel> claimsToAssign)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimBindingModel claimModel in claimsToAssign)
            {
                if (appUser.Claims.Any(c => c.ClaimType == claimModel.Type))
                {

                    await this.AppUserManager.RemoveClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
                }

                await this.AppUserManager.AddClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
            }

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/removeclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> RemoveClaimsFromUser([FromUri] string id, [FromBody] List<ClaimBindingModel> claimsToRemove)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimBindingModel claimModel in claimsToRemove)
            {
                if (appUser.Claims.Any(c => c.ClaimType == claimModel.Type))
                {
                    await this.AppUserManager.RemoveClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
                }
            }

            return Ok();
        }

    }
}