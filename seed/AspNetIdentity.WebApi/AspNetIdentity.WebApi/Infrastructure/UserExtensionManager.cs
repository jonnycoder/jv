using MySql.AspNet.Identity;
using MySql.Data.Entity;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AspNetIdentity.WebApi.Infrastructure
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public partial class UserExtensionContext : DbContext
    {
        public UserExtensionContext() : base(nameOrConnectionString: "DefaultConnection") { }
        public DbSet<UserExtension> UserExtensions { get; set; }
        public Task<int> Update()
        {
            Task<int> result = null;
            try
            {
                result = this.SaveChangesAsync();
            }
            finally
            {

            }

            return result;
        }
    }

    public class UserExtensionManager
    {
        private static UserExtensionContext _theContext;
        private UserExtensionManager()
        {

        }

        public static UserExtensionContext Context
        {
            get
            {
                if (null == _theContext)
                {
                    _theContext = new UserExtensionContext();
                }

                return _theContext;
            }
        }
    }
}