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
    public class Program
    {
        [Key]
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}