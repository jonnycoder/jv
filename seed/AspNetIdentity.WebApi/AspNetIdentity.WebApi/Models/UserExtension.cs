using MySql.AspNet.Identity;
using MySql.Data.Entity;
using MySql.Data;
using System.Data.Linq.Mapping;
using System.ComponentModel.DataAnnotations;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetIdentity.WebApi.Models
{
    public class UserExtension
    {
        [Key]
        public string UserId { get; set; }

        public string SkypeHandle { get; set; }

        public string IndividualDescription { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public int Credits { get; set; }

        public int Category { get; set; }
    }

    public class MarketerUser
    {
        [Key]
        [Column(Name = "Id")]
        public string UserId { get; set; }
    }

    public class ProgramUnlock
    {
        [Key]
        public string PayingUser { get; set; }

        public string ProgramName { get; set; }
    }

    public class UserRating
    {
        [Key]
        public long Id { get; set; }

        public string Rater { get; set; }
        public string Rated { get; set; }
        public int Rating { get; set; }

    }

    public class AffiliateUnlock
    {
        [Key]
        public string PayingUser { get; set; }

        public string RevealedUser { get; set; }

        public int RevealedRating { get; set; }
    }

    public class AffilateUser
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string SkypeHandle { get; set; }

        public string IndividualDescription { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }
    }

}