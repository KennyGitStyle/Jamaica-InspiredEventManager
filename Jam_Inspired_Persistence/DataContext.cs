using Jam_Inspired_Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Jam_Inspired_Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) :
            base(options)
        {
        }

        public DbSet<Value> Values { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Value>()
                .HasData(new Value { Id = 1, Name = "Kenneth" },
                new Value { Id = 2, Name = "Aaron" },
                new Value { Id = 3, Name = "Ashley" });

            modelBuilder.Entity<UserActivity>(x => x.HasKey(ua => 
                new {ua.AppUserId, ua.ActivityId}));

            modelBuilder.Entity<UserActivity>().HasOne(u => u.AppUser)
                .WithMany(a => a.UserActivities).HasForeignKey(u => u.AppUserId);


            modelBuilder.Entity<UserActivity>().HasOne(a => a.Activity)
                .WithMany(u => u.UserActivities).HasForeignKey(a => a.ActivityId);
        }
    }
}
