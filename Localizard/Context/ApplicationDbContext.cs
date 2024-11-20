using Localizard.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Xml;
using Localizard.Domain.Entites;
using Permission = Localizard.Models.Permission;


namespace Localizard._context
{
    public class ApplicationDbContext : DbContext
    {
       

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseNpgsql("Host = localhost; Database = myprojectdb; Username = postgres; Password = Boburjon2002")
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging();
            }
        }
        

     
        public DbSet<Language> Languages { get; set; }
        public DbSet<ProjectInfo> Projects { get; set; }
        public DbSet<ProjectDetail> ProjectDetails{ get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<RegisterModel> RegisterModels { get; set; }
        public DbSet<LoginModel> LoginModels { get; set; }
      
     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Role>().HasData(
           new Role { Id = 1, Name = "User" },
           new Role { Id = 2, Name = "Admin" }
       );
           
          


            modelBuilder.Entity<LoginModel>().HasNoKey();
            modelBuilder.Entity<RegisterModel>().HasNoKey();
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new {rp.RoleId, rp.PermissionId});

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany()
                .HasForeignKey(rp => rp.PermissionId);

            modelBuilder.Entity<User>()
            .HasOne<Role>() 
            .WithOne() 
            .HasForeignKey<Role>(r => r.UserId) 
            .IsRequired();

              modelBuilder.Entity<User>()
             .HasOne<Role>() 
             .WithOne(r => r.User) 
             .HasForeignKey<Role>(r => r.UserId) 
             .IsRequired();
        }
    }
}