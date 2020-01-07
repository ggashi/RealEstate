using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstate.WebUI.Models;

namespace RealEstate.WebUI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Type>().HasData(new Type { Id = 1, Name = "Apartment" });
            modelBuilder.Entity<Type>().HasData(new Type { Id = 2, Name = "House" });
            modelBuilder.Entity<Type>().HasData(new Type { Id = 3, Name = "Room" });

            modelBuilder.Entity<Location>().HasData(new Location { Id = 1, Name = "Prishtina" });
            modelBuilder.Entity<Location>().HasData(new Location { Id = 2, Name = "Mitrovica" });
            modelBuilder.Entity<Location>().HasData(new Location { Id = 3, Name = "Peje" });
            modelBuilder.Entity<Location>().HasData(new Location { Id = 4, Name = "Prizren" });
            modelBuilder.Entity<Location>().HasData(new Location { Id = 5, Name = "Ferizaj" });
            modelBuilder.Entity<Location>().HasData(new Location { Id = 6, Name = "Gjilan" });
            modelBuilder.Entity<Location>().HasData(new Location { Id = 7, Name = "Gjakove" });
        }

        public DbSet<Type> Types { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<UserProperty>UserProperties { get; set; }
        public DbSet <Contact> Contacts { get; set; }
    }
}
