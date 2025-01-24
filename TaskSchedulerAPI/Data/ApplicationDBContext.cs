using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskSchedulerAPI.Model;

namespace TaskSchedulerAPI.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions <ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<StatusModel> StatusModel { get; set; }
        public DbSet<TaskLevelModel> TaskLevel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StatusModel>().HasData(
                new StatusModel { Id = 1 , Name = "Pending"},
                new StatusModel { Id = 2, Name = "Done" },
                new StatusModel { Id = 3, Name = "Expired" }
                );

            modelBuilder.Entity<TaskLevelModel>().HasData(
               new TaskLevelModel { Id = 1, Level = "Priority" },
               new TaskLevelModel { Id = 2, Level = "High" },
               new TaskLevelModel { Id = 3, Level = "Low" }
               );

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },

                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                }

            };
            
            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
