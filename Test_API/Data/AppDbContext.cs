using Microsoft.EntityFrameworkCore;
using Test_API.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Test_API.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Category> Categoryes { get; set; } // Corrected the property name
        public DbSet<Item> Items { get; set; }

        //public DbSet<AppUser> AppUsers { get; set; }


    }
}
