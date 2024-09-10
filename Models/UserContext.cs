using Microsoft.EntityFrameworkCore;

namespace TokenGenerationApi.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options):base(options)
        {

        }

        public DbSet<LoginModel>? LoginModels { get; set; }
      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<LoginModel>().HasData(
            //    new LoginModel
            //{
            //        Id=1,
            //    UserName = "raghava",
            //    Password = "#raghava2409",
            //   Roles=new List<Roles>
            //   {
            //       new Roles{RoleName="Student",LoginModel=new LoginModel()
            //       { 
            //            Id=1
            //       } },
            //       new Roles{RoleName="Admin",LoginModel=new LoginModel()
            //       {
            //         Id=1
            //       }
            //       }
            //   }
            //},
            //    new LoginModel
            //    {
            //        Id = 2,
            //        UserName = "Kiran",
            //        Password = "#kiran2409",
            //        Roles = new List<Roles>
            //   {
            //       new Roles{RoleName="Student",LoginModel=new LoginModel()
            //       {
            //           Id=2
            //       }               }
            //    }
            //    });
           
        }
    }
}
