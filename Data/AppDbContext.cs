using Microsoft.EntityFrameworkCore;
using ProdApi.Models;

namespace ProdApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }
        public DbSet<Product> Products { get; set; }    

    }
}
