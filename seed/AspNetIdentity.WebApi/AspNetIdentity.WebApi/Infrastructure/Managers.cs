using MySql.AspNet.Identity;
using MySql.Data.Entity;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
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
        public DbSet<AffiliateUnlock> UserUserUnlocks { get; set; }
        public DbSet<ProgramUnlock> UserProgramUnlocks { get; set; }
        public DbSet<UserRating> UserRatings { get; set; }

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
                    _theContext.Database.Log = s => System.Diagnostics.Debug.WriteLine("Entites Query: " +s);
                }

                return _theContext;
            }
        }

        //protected override void OnModelCreating(
        //                    DbModelBuilder modelBuilder)
        //{
        //   // modelBuilder.Configurations.Add(new AspnetUserRoleConfiguration());
        //    // ...  and so on, for each configuration class

        //    base.OnModelCreating(modelBuilder);
        //}

        //public class AspnetUserRoleConfiguration : EntityTypeConfiguration<AspnetUserRole>
        //{
        //    public AspnetUserRoleConfiguration()
        //    {
        //        HasKey(a => a.UserId);
        //        HasKey(a => a.RoleId);
        //    }
        //}
    }

    public class UserRatingManager
    {
        private UserRatingManager()
        {

        }

        private static JVContext Context
        {
            get { return JVContext.Instance; }
        }

        public static bool RegisterRating(
                string raterUserid,
                string affiliateId,
                int rating)
        {
            try {
                UserRating lookup = UserRatingManager.UserRatings.Where(r => r.Rated == affiliateId && r.Rater == raterUserid).FirstOrDefault();
                if (null == lookup)
                {
                    lookup = new UserRating();
                    lookup.Rater = raterUserid;
                    lookup.Rated = affiliateId;
                    UserRatingManager.UserRatings.Add(lookup);
                }
                lookup.Rating = rating;
                UserRatingManager.Update();
            }
            catch { return false; }

            return true;
        }
         

        public static Task<int> Update()
        {
            Task<int> result = null;
            try
            {
                result = UserRatingManager.Context.SaveChangesAsync();
            }
            finally
            {

            }

            return result;
        }

        public static DbSet<UserRating> UserRatings { get { return UserRatingManager.Context.UserRatings; } }


    }
   
    public class UserExtensionManager
    {

        private UserExtensionManager()
        {

        }

        private static JVContext Context
        {
            get { return JVContext.Instance; }
        }

        public static Task<int> Update()
        {
            Task<int> result = null;
            try
            {
                result = UserExtensionManager.Context.SaveChangesAsync();
            }
            finally
            {

            }

            return result;
        }

        public static DbSet<UserExtension> UserExtensions { get { return UserExtensionManager.Context.UserExtensions; } }
    }

    public class MarketManager
    {
        private MarketManager()
        {

        }

        private static JVContext Context
        {
            get { return JVContext.Instance; }
        }

        public static Task<int> Update()
        {
            Task<int> result = null;
            try
            {
                result = MarketManager.Context.SaveChangesAsync();
            }
            finally
            {

            }

            return result;
        }

        public static IEnumerable<Program> GetAllPrograms()
        {
            return MarketManager.Context.Programs;
        }

        public static IEnumerable<ProgramUnlock> GetUnlockedPrograms(string forUser)
        {
            return MarketManager.Context.UserProgramUnlocks.SqlQuery(String.Format("Select * from userprogramunlocks where payinguser = '{0}'", forUser));
        }

        public static IEnumerable<AffilateUser> GetAllAffiliates()
        {
            return MarketManager.Context.Affiliates.SqlQuery("Select * from affiliateusers");
        }

        public static IEnumerable<AffiliateUnlock> GetUnlockedAffiliates(string forUser)
        {
            return MarketManager.Context.UserUserUnlocks.SqlQuery(String.Format("Select * from useruserunlocks where payinguser = '{0}'", forUser));
        }


        public static List<AffiliateReturnModel> GetAffiliates()
        {
            IEnumerable<AffiliateReturnModel> affiliates = MarketManager.GetAllAffiliates().ToList().Select(a => new AffiliateReturnModel { IndividualDescription = a.IndividualDescription, UserId = a.Id });

            return affiliates.ToList();
        }

        public static List<AffiliateReturnModel> UnlockedAffiliates(string forUser)
        {
            IEnumerable<AffiliateUnlock> unlocked = MarketManager.GetUnlockedAffiliates(forUser).ToList();

            IEnumerable<AffiliateReturnModel> affiliates = MarketManager.GetAllAffiliates().Join(unlocked,
                outerKey => outerKey.Id,
                innerKey => innerKey.RevealedUser,
                (a, u) => new AffiliateReturnModel { Email = a.Email, FirstName = a.FirstName, IndividualDescription = a.IndividualDescription, LastName = a.LastName, PhoneNumber = a.PhoneNumber, SkypeHandle = a.SkypeHandle, Username = a.UserName, Rating = u.RevealedRating, UserId = u.RevealedUser });

            // on.Where(a => unlocked.Select(u => u.RevealedUser).Contains(a.Id)).ToList().Select(a => new AffiliateReturnModel { Email = a.Email, FirstName = a.FirstName, IndividualDescription = a.IndividualDescription, LastName = a.LastName, PhoneNumber = a.PhoneNumber, SkypeHandle = a.SkypeHandle, Username = a.UserName, Rating = });

            return affiliates.ToList();
        }

        public static List<ProgramReturnModel> GetPrograms()
        {
            IEnumerable<ProgramReturnModel> programs = MarketManager.Programs.ToList().Select(p => new ProgramReturnModel { CreatedDate = p.CreatedDate.ToShortDateString(), ProgramDescription = p.Description, ProgramName = p.Name });

            return programs.ToList();
        }

        public static List<ProgramReturnModel> UnlockedPrograms(string forUser, ApplicationUserManager appUserManager)
        {
            // get the list of user revealed programs
            IEnumerable<ProgramUnlock> unlocked = MarketManager.GetUnlockedPrograms(forUser);
            IEnumerable<string> unlockedProgramNames = unlocked.Select(p => p.ProgramName);
            // get those programs
            IEnumerable<Program> unlockedPrograms = MarketManager.Programs.Where(p => unlockedProgramNames.Contains(p.Name));

            // and fill in with the contact info for the program creator
            IEnumerable<ProgramReturnModel> returnPrograms = from p in unlockedPrograms
                                                             join ue in UserExtensionManager.UserExtensions on p.CreatorId equals ue.UserId
                                                             select
                                                             new ProgramReturnModel { CreatedDate = p.CreatedDate.ToShortDateString(), ProgramDescription = p.Description, ProgramName = p.Name, ProgramUrl = p.Url, CreatorFullName = ue.FirstName + " " + ue.LastName, CreatorPhoneNumber = ue.PhoneNumber, CreatorSkypeHandle = ue.SkypeHandle };

            return returnPrograms.ToList();
        }

        public static DbSet<Program> Programs { get { return MarketManager.Context.Programs; } }
        public static DbSet<AffilateUser> Affiliates { get { return MarketManager.Context.Affiliates; } }
        public static DbSet<MarketerUser> Marketers { get { return MarketManager.Context.Marketers; } }
    }

    public class AspNetRoleManager
    {
        private AspNetRoleManager()
        {

        }

        private static JVContext Context
        {
            get { return JVContext.Instance; }
        }

        public static Task<int> Update()
        {
            Task<int> result = null;
            try
            {
                result = AspNetRoleManager.Context.SaveChangesAsync();
            }
            finally
            {

            }

            return result;
        }

    }
}