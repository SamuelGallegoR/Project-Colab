using Lab1.Areas.ProjectManagement.Models;
using Lab1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Lab1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> // Using Identity
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<ProjectComment> ProjectComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure Identity's model creating is called

            modelBuilder.Entity<ProjectTask>()
                .HasOne(pt => pt.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(pt => pt.ProjectID);

            /*
            modelBuilder.HasDefaultSchema("Identity");

            */

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User");

            });
            

            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");

            });

            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable(name: "UserRoles");

            });

            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable(name: "UserClaims");

            });

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable(name: "UserLogins");

            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable(name: "RoleClaims");

            });


            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable(name: "UserTokens");

            });

        }

        
    }
}
