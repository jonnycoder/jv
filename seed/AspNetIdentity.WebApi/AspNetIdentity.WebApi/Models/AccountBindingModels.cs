using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspNetIdentity.WebApi.Models
{
    public class CreateUserBindingModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(32, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Skype handle")]
        public string SkypeHandle { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [DataType(DataType.Text)]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Affiliate")]
        public string Affiliate { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Affiliate")]
        public string Marketer { get; set; }

        [DataType(DataType.Url)]
        [RegularExpression(@"^http(s)?\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$", ErrorMessage ="Please enter a valid url")]
        [Display(Name = "Program url")]
        public string ProgramUrl { get; set; }

        [StringLength(50, ErrorMessage = "Please enter a {0} at least {2} characters long.", MinimumLength = 10)]
        [DataType(DataType.Text)]
        [Display(Name = "Program name")]
        public string ProgramName { get; set; }

        [StringLength(2000, ErrorMessage = "Please enter a {0} at least {2} characters long.", MinimumLength = 40)]
        [DataType(DataType.Text)]
        [Display(Name = "Program description")]
        public string ProgramDescription { get; set; }

        [StringLength(2000, ErrorMessage = "Please enter a {0} at least {2} characters long.", MinimumLength = 40)]
        [DataType(DataType.Text)]
        [Display(Name = "Individual description")]
        public string IndividualDescription { get; set; }
    }

    public class ChangePasswordBindingModel
    {
       
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    
    }

    public class LoginUserBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "User name")]
        public string UserName { get; set; }

    }
}