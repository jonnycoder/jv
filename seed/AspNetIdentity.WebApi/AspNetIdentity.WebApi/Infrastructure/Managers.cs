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
        public JVContext() : base(nameOrConnectionString: "DefaultConnection")
        {
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine("Entites Query: " + s);
        }
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
        private JVContext Context;
        public UserRatingManager(JVContext context)
        {
            Context = context;
        }

        public int AffiliateAverageRating(
                string affiliateId)
        {
            double rating = Context.UserRatings.Where(r => r.Rated == affiliateId).Select(r => r.Rating).DefaultIfEmpty(0).Average();

            return Convert.ToInt32(Math.Round(rating));
        }

        public int MyRatingOf(string ratingUser, string ratedUser)
        {
            UserRating rating = Context.UserRatings.Where(r => r.Rated == ratedUser && r.Rater == ratingUser).FirstOrDefault();
            if (null == rating)
            { return 0; }

            return rating.Rating;
        }

        public  bool RegisterRating(
                string raterUserid,
                string affiliateId,
                int rating)
        {
            try
            {
                UserRating lookup = Context.UserRatings.Where(r => r.Rated == affiliateId && r.Rater == raterUserid).FirstOrDefault();
                if (null == lookup)
                {
                    lookup = new UserRating();
                    lookup.Rater = raterUserid;
                    lookup.Rated = affiliateId;
                    Context.UserRatings.Add(lookup);
                }
                lookup.Rating = rating;
                Update();
            }
            catch { return false; }

            return true;
        }


        public Task<int> Update()
        {
            Task<int> result = null;
            try
            {
                result = Context.SaveChangesAsync();
            }
            finally
            {

            }

            return result;
        }

        public DbSet<UserRating> UserRatings { get { return Context.UserRatings; } }


    }

    public class UserExtensionManager
    {
        private JVContext Context;
        public UserExtensionManager(JVContext context)
        {
            Context = context;
        }

        public Task<int> Update()
        {
            Task<int> result = null;
            try
            {
                result = Context.SaveChangesAsync();
            }
            finally
            {

            }

            return result;
        }

        public DbSet<UserExtension> UserExtensions { get { return Context.UserExtensions; } }
    }

    public class MarketManager
    {
        private JVContext Context;
        private UserRatingManager RatingManager;
        public MarketManager(JVContext context, UserRatingManager ratingManager)
        {
            Context = context;
            RatingManager = ratingManager;
        }

        public Task<int> Update()
        {
            Task<int> result = null;
            try
            {
                result = Context.SaveChangesAsync();
            }
            finally
            {

            }

            return result;
        }

        public IEnumerable<Program> GetAllPrograms()
        {
            return Context.Programs;
        }

        public IEnumerable<ProgramUnlock> GetUnlockedPrograms(string forUser)
        {
            return Context.UserProgramUnlocks.SqlQuery(String.Format("Select * from userprogramunlocks where payinguser = '{0}'", forUser));
        }

        public IEnumerable<AffilateUser> GetAllAffiliates()
        {
            return Context.Affiliates.SqlQuery("Select * from affiliateusers");
        }

        public IEnumerable<AffiliateUnlock> GetUnlockedAffiliates(string forUser)
        {
            return Context.UserUserUnlocks.SqlQuery(String.Format("Select * from useruserunlocks where payinguser = '{0}'", forUser));
        }

        public void AddUnlockedAffiliate(string reveal, string forUser)
        {
            Context.Database.ExecuteSqlCommand(String.Format("Insert Into useruserunlocks(PayingUser, RevealedUser) values('{0}', '{1}')", forUser, reveal));
        }

        public void AddUnlockedProgram(string reveal, string forUser)
        {
            Context.Database.ExecuteSqlCommand(String.Format("Insert Into userprogramunlocks(PayingUser, ProgramName) values('{0}', '{1}')", forUser, reveal));
        }

        public List<AffiliateReturnModel> GetAffiliates()
        {
            IEnumerable<AffiliateReturnModel> affiliates = GetAllAffiliates().ToList().Select(a => new AffiliateReturnModel { IndividualDescription = a.IndividualDescription, UserId = a.Id, CategoryDescription = a.CategoryName });

            return affiliates.ToList();
        }

        public bool RevealUserFor(string requestingUser, string revealUser)
        {
            AffiliateUnlock lookup = GetUnlockedAffiliates(requestingUser).Where(u => u.RevealedUser == revealUser).FirstOrDefault();

            if (null == lookup)
            {
                AffiliateUnlock newUnlock = new AffiliateUnlock();
                newUnlock.PayingUser = requestingUser;
                newUnlock.RevealedUser = revealUser;
                try
                {
                    UserExtension requestingUserDetails = Context.UserExtensions.Where(u => u.UserId == requestingUser).FirstOrDefault();
                    if (null == requestingUserDetails || requestingUserDetails.Credits == 0)
                    {
                        return false;
                    }

                    AddUnlockedAffiliate(revealUser, requestingUser);
                    requestingUserDetails.Credits--;
                    Update();
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }
        public bool RevealProgramFor(string requestingUser, string revealProgram)
        {
            ProgramUnlock lookup = GetUnlockedPrograms(requestingUser).Where(u => u.ProgramName == revealProgram).FirstOrDefault();

            if (null == lookup)
            {
                ProgramUnlock newUnlock = new ProgramUnlock();
                newUnlock.PayingUser = requestingUser;
                newUnlock.ProgramName = revealProgram;
                try
                {
                    UserExtension requestingUserDetails = Context.UserExtensions.Where(u => u.UserId == requestingUser).FirstOrDefault();
                    if (null == requestingUserDetails || requestingUserDetails.Credits == 0)
                    {
                        return false;
                    }

                    AddUnlockedProgram(revealProgram, requestingUser);
                    requestingUserDetails.Credits--;
                    Update();
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        public List<AffiliateReturnModel> UnlockedAffiliates(string forUser)
        {
            IEnumerable<AffiliateUnlock> unlocked = GetUnlockedAffiliates(forUser).ToList();

            IEnumerable<AffiliateReturnModel> affiliates = GetAllAffiliates().Join(unlocked,
                outerKey => outerKey.Id,
                innerKey => innerKey.RevealedUser,
                (a, u) => new AffiliateReturnModel { Email = a.Email, FirstName = a.FirstName, IndividualDescription = a.IndividualDescription, LastName = a.LastName, PhoneNumber = a.PhoneNumber, SkypeHandle = a.SkypeHandle, Username = a.UserName, UserId = u.RevealedUser, CategoryDescription = a.CategoryName }).ToList();

            foreach (AffiliateReturnModel a in affiliates)
            {
                a.MyRating = RatingManager.MyRatingOf(forUser, a.UserId);
                a.AvgRating = RatingManager.AffiliateAverageRating(a.UserId);
            }

            return affiliates.ToList();
        }

        public List<ProgramReturnModel> GetPrograms()
        {
            IEnumerable<Category> categories = Context.Categories.ToList();
            IEnumerable<ProgramReturnModel> programs = Programs.ToList().Join(categories,
                outerKey => outerKey.Category,
                innerKey => innerKey.Id,
                (p, c) => new ProgramReturnModel { CreatedDate = p.CreatedDate.ToShortDateString(), ProgramDescription = p.Description, ProgramName = p.Name, ProgramCategory = c.Name }).ToList();

            return programs.ToList();
        }

        public List<ProgramReturnModel> UnlockedPrograms(string forUser)
        {
            // get the list of user revealed programs
            IEnumerable<ProgramUnlock> unlocked = GetUnlockedPrograms(forUser).ToList();
            IEnumerable<string> unlockedProgramNames = unlocked.Select(p => p.ProgramName).ToList();
            // get those programs
            IEnumerable<Program> unlockedPrograms = Programs.Where(p => unlockedProgramNames.Contains(p.Name)).ToList();

            // and fill in with the contact info for the program creator
            IEnumerable<ProgramReturnModel> returnPrograms = from p in unlockedPrograms
                                                             join ue in Context.UserExtensions on p.CreatorId equals ue.UserId
                                                             select
                                                             new ProgramReturnModel { CreatedDate = p.CreatedDate.ToShortDateString(), ProgramDescription = p.Description, ProgramName = p.Name, ProgramUrl = p.Url, CreatorFullName = ue.FirstName + " " + ue.LastName, CreatorPhoneNumber = ue.PhoneNumber, CreatorSkypeHandle = ue.SkypeHandle };

            return returnPrograms.ToList();
        }

        public DbSet<Program> Programs { get { return Context.Programs; } }
        public DbSet<AffilateUser> Affiliates { get { return Context.Affiliates; } }
        public DbSet<MarketerUser> Marketers { get { return Context.Marketers; } }
    }

   
    public class LookupDataManager
    {
        private JVContext Context;
        public LookupDataManager(JVContext context)
        {
            Context = context;
        }

        public DbSet<Category> Categories { get { return Context.Categories; } }
    }
}