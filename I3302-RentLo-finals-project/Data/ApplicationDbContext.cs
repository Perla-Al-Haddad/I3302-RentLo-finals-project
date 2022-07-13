using I3302_RentLo_finals_project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace I3302_RentLo_finals_project.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<I3302_RentLo_finals_project.Models.Property> Property { get; set; }

        public DbSet<UserPropertyRent> UserPropertyRents { get; set; }

        public DbSet<Image> Images { get; set; }
    }
}