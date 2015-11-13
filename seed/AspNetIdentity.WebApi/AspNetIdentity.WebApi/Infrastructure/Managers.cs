using MySql.AspNet.Identity;
using MySql.Data.Entity;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using AspNetIdentity.WebApi.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace AspNetIdentity.WebApi.Infrastructure
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public partial class JVContext : DbContext
    {
        private static JVContext _theContext;
        public JVContext() : base(nameOrConnectionString: "DefaultConnection") { }
        public DbSet<UserExtension> UserExtensions { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<MarketerUser> Marketers { get; set; }
        public DbSet<AffilateUser> Affiliates { get; set; }
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

        public static JVContext Instance
        {
            get
            {
                if (null == _theContext)
                {
                    _theContext = new JVContext();
                }

                return _theContext;
            }
        }
    }

    public class UserExtensionManager
    {

        private UserExtensionManager()
        {

        }

        public static JVContext Context
        {
            get { return JVContext.Instance; }
        }
       
    }

    public class MarketManager
    {
        private MarketManager()
        {

        }

        public static JVContext Context
        {
            get { return JVContext.Instance; }
        }

        public static DbSet<Program> Programs { get { return MarketManager.Context.Programs; } }
        public static DbSet<AffilateUser> Affiliates { get { return MarketManager.Context.Affiliates; } }
        public static DbSet<MarketerUser> Marketers { get { return MarketManager.Context.Marketers; } }
    }
}