#pragma warning disable 8618

using Microsoft.EntityFrameworkCore;

using DAL_Layer.Model;

namespace DAL_Layer
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        public DbSet<UserInterest> UserInterests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserEvent>().ToTable("UserEvents");
            modelBuilder.Entity<UserInterest>().ToTable("UserInterests");

            modelBuilder.Entity<User>()
                .HasMany(x => x.Events)
               .WithOne(x => x.User);
            modelBuilder.Entity<User>()
                .HasMany(x => x.Interests)
                .WithOne(x => x.User);
        }
    }
}