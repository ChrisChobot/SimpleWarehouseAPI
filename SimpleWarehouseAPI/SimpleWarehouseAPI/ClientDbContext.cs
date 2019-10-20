using Microsoft.EntityFrameworkCore;
using SimpleWarehouseAPI.Models;

namespace SimpleWarehouseAPI
{
    public class ClientDbContext : DbContext
    {
        const string connectionString = "Server=Localhost;Database=SimpleWarehouseDB;Trusted_Connection=True;";

        public ClientDbContext() : base() { }

        public ClientDbContext(DbContextOptions<ClientDbContext> options) : base(options) { }

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
