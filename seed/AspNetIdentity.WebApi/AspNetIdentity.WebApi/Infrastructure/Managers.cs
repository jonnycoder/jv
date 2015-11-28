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
        public DbSet<Category> Categories { get; set; }

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
                    _theContext.Database.Log = s => System.Diagnostics.Debug.WriteLine("Entites Query: " + s);
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


        public static int AffiliateAverageRating(
                string affiliateId)
        {
            double rating = UserRatingManager.Context.UserRatings.Where(r => r.Rated == affiliateId).Select(r => r.Rating).DefaultIfEmpty(0).Average();

            return Convert.ToInt32(Math.Round(rating));
        }

        public static int MyRatingOf(string ratingUser, string ratedUser)
        {
            UserRating rating = UserRatingManager.Context.UserRatings.Where(r => r.Rated == ratedUser && r.Rater == ratingUser).FirstOrDefault();
            if (null == rating)
            { return 0; }

            return rating.Rating;
        }

        public static bool RegisterRating(
                string raterUserid,
                string affiliateId,
                int rating)
        {
            try
            {
                UserRating lookup = UserRatingManager.UserRatings.Where(r => r.Rated == affiliateId && r.Rater == raterUserid).FirstOrDefault();
                if (null == lookup)
                {
                    lookup = new UserRating();
                    lookup.Rater = raterUserid;
                    lookup.Rated = affiliateId;
                    UserRatingManager.Context.UserRatings.Add(lookup);
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

        public static void AddUnlockedAffiliate(string reveal, string forUser)
        {
            MarketManager.Context.Database.ExecuteSqlCommand(String.Format("Insert Into useruserunlocks(PayingUser, RevealedUser) values('{0}', '{1}')", forUser, reveal));
        }


        public static List<AffiliateReturnModel> GetAffiliates()
        {
            IEnumerable<AffiliateReturnModel> affiliates = MarketManager.GetAllAffiliates().ToList().Select(a => new AffiliateReturnModel { IndividualDescription = a.IndividualDescription, UserId = a.Id, CategoryDescription = a.CategoryName });

            return affiliates.ToList();
        }

        public static bool RevealFor(string requestingUser, string revealUser) {
            AffiliateUnlock lookup = MarketManager.GetUnlockedAffiliates(requestingUser).Where(u => u.RevealedUser == revealUser).FirstOrDefault();

            if (null == lookup)
            {
                AffiliateUnlock newUnlock = new AffiliateUnlock();
                newUnlock.PayingUser = requestingUser;
                newUnlock.RevealedUser = revealUser;
                try
                {
                    MarketManager.AddUnlockedAffiliate(revealUser, requestingUser);
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        public static List<AffiliateReturnModel> UnlockedAffiliates(string forUser)
        {
            IEnumerable<AffiliateUnlock> unlocked = MarketManager.GetUnlockedAffiliates(forUser).ToList();

            IEnumerable<AffiliateReturnModel> affiliates = MarketManager.GetAllAffiliates().Join(unlocked,
                outerKey => outerKey.Id,
                innerKey => innerKey.RevealedUser,
                (a, u) => new AffiliateReturnModel { Email = a.Email, FirstName = a.FirstName, IndividualDescription = a.IndividualDescription, LastName = a.LastName, PhoneNumber = a.PhoneNumber, SkypeHandle = a.SkypeHandle, Username = a.UserName, UserId = u.RevealedUser, CategoryDescription = a.CategoryName }).ToList();

            foreach (AffiliateReturnModel a in affiliates)
            {
                a.MyRating = UserRatingManager.MyRatingOf(forUser, a.UserId);
                a.AvgRating = UserRatingManager.AffiliateAverageRating(a.UserId);
            }

            return affiliates.ToList();
        }

        public static List<ProgramReturnModel> GetPrograms()
        {
            IEnumerable<Category> categories = LookupDataManager.Categories.ToList();
            IEnumerable<ProgramReturnModel> programs = MarketManager.Programs.ToList().Join(categories,
                outerKey => outerKey.Category,
                innerKey => innerKey.Id,
                (p, c) => new ProgramReturnModel { CreatedDate = p.CreatedDate.ToShortDateString(), ProgramDescription = p.Description, ProgramName = p.Name, ProgramCategory = c.Name }).ToList();

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

    public class LookupDataManager
    {
        private LookupDataManager()
        {

        }

        private static JVContext Context
        {
            get { return JVContext.Instance; }
        }

        public static DbSet<Category> Categories { get { return LookupDataManager.Context.Categories; } }
    }
}