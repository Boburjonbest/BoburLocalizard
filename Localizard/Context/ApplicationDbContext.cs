using Localizard.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Xml;
using static Localizard.Models.Language;




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






        public DbSet<Tag> Tags { get; set; }
        public DbSet<EmpClass> EmpClasses { get; set; }

        public DbSet<Project> MyEntities { get; set; }



        public DbSet<Language> languages { get; set; }

     

        public DbSet<User> Users { get; set; }
       public DbSet<Role> Roles { get; set; }
       public DbSet<Permission> Permissions { get; set; }
       public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<RegisterModel> RegisterModels { get; set; }
        public DbSet<LoginModel> LoginModels { get; set; }
        public DbSet<ObyektPerevod> ObyektPerevods { get; set; }
        public DbSet<ObyektTranslation> ObyektTranslations { get; set; }
     








        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Role>().HasData(
           new Role { Id = 1, Name = "User" },
           new Role { Id = 2, Name = "Admin" }
       );
            //var converter = new ValueConverter<TranslationContent, string>(
            //    v => JsonConvert.SerializeObject(v), 
            //    v => JsonConvert.DeserializeObject<TranslationContent>(v)); 

            //modelBuilder.Entity<Perevod>()
            //    .HasKey(p => new { p.Id, p.Name, p.Russian, p.English, p.ParentId });

          

            modelBuilder.Entity<Language>()
                .ToTable("languages")
                .Property(l => l.PluralForms)
                .HasColumnType("text[]");

            modelBuilder.Entity<Language>()
                .HasKey(l => l.Id);


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
            

            modelBuilder.Entity<Project>()
           .HasKey(p => p.Id);

            modelBuilder.Entity<Project>()
             .Property(p => p.AvailableLanguage)
              .HasColumnType("text[]");

            modelBuilder.Entity<User>()
                 .HasIndex(u => u.Username)
                 .IsUnique();

            modelBuilder.Entity<Language>()
            .HasKey(l => new { l.Name, l.LanguageCode });

        



































            //modelBuilder.Entity<Perevod>()
            //    .Ignore(p => p.Perevods);
            //modelBuilder.Entity<Perevod>()
            //    .HasKey(p => p.Id);
            //modelBuilder.Entity<PerevodDetails>()
            //    .HasNoKey();
            //base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Perevod>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Name).IsRequired();
            //entity.Property(e => e.Russian).IsRequired();
            //entity.Property(e => e.English).IsRequired();
            //entity.Property(e => e.ParentId).IsRequired(false);

            //});































            /*public override int SaveChanges()
            {
                foreach (var entry in ChangeTracker.Entries<MyEntity>())
                {
                    if (entry.State == EntityState.Modified)
                    {
                        entry.Entity.UpdatedAt = DateTime.Now;

                    }
                    else if (entry.State == EntityState.Added)
                    {
                        entry.Entity.CreatedAt = DateTime.Now;
                        entry.Entity.UpdatedAt = DateTime.Now;
                    }
                }
                return base.SaveChanges();
            }*/



        }
    }
}