using Microsoft.EntityFrameworkCore;
using SimpleWarehouseAPI.Models;

namespace SimpleWarehouseAPI
{
    public class MainDbContext : DbContext
    {
        const string connectionString = "Server=Localhost;Database=SimpleWarehouseDB;Trusted_Connection=True;";

        public MainDbContext() : base() { }

        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }            
        }
    }
}
