using KitchenKeeper.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KitchenKeeper.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<JSON> JSONs { get; set; }
        public DbSet<PDF> PDFs { get; set; }
        public DbSet<StorageLocation> StorageLocations { get; set; }
        public DbSet<User> Users { get; set; }

    }
}