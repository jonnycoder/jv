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

namespace AspNetIdentity.WebApi.Infrastructure
{
    [Table(Name = "user_extension")]
    public class UserExtension
    {
        [Key]
        [Column(Name = "UserId")]
        public string userId { get; set; }

        public string SkypeHandle { get; set; }
    }
}