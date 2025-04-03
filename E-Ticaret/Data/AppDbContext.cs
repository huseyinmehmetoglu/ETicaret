using Microsoft.EntityFrameworkCore; 
using ETicaret.Models;

namespace ETicaret.Data
{
    public class AppDbContext : DbContext // DbContext: veritabanı işlemlerimin merkezi
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) 
        { }

        public DbSet<Product> Products { get; set; } // Ürün tablosu
        public DbSet<Category> Categories { get; set; } // Kategori tablosu

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)"); // Fiyat sütununu decimal(18,2) olarak ayarlıyorum

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Elektronik" }, 
                new Category { CategoryId = 2, Name = "Giyim" },       
                new Category { CategoryId = 3, Name = "Ev Eşyaları" }   
            );
        }
    }
}
