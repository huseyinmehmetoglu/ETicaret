using Microsoft.EntityFrameworkCore;
using ETicaret.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ETicaret.Data
{
    public class AppDbContext : DbContext // DbContext: veritabanı işlemlerimin merkezi
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) // Bağlantı seçeneklerini üst sınıfa geçiriyorum
        { }

        public DbSet<Product> Products { get; set; } // Ürün tablosu
        public DbSet<Category> Categories { get; set; } // Kategori tablosu

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)"); // Fiyat sütununu decimal(18,2) olarak ayarlıyorum

            // Seed Data: Projeyi test etmek için başlangıç kategorileri ekliyorum
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Elektronik" }, // Örnek kategori 1
                new Category { CategoryId = 2, Name = "Giyim" },       // Örnek kategori 2
                new Category { CategoryId = 3, Name = "Ev Eşyaları" }    // Örnek kategori 3
            );
        }
    }
}