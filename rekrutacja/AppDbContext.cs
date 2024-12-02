using Microsoft.EntityFrameworkCore;
using rekrutacja.Entities;

namespace rekrutacja
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            modelBuilder.Entity<City>()
                .HasIndex(c => c.Name)
                .IsUnique(); 
        }
    }
}
