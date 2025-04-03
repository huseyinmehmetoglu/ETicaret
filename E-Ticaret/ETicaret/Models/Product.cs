using System.ComponentModel.DataAnnotations; 

namespace ETicaret.Models
{
    public class Product 
    {
        public int ProductId { get; set; } 
        [Required(ErrorMessage = "Ürün adı zorunludur.")] public string Name { get; set; } // Ürün adı, zorunlu
        [Required(ErrorMessage = "Fiyat zorunludur.")] public decimal Price { get; set; } // Ürün fiyatı, zorunlu
        [Required(ErrorMessage = "Lütfen bir kategori seçiniz.")] public int CategoryId { get; set; } // Kategori seçimi, zorunlu
        public Category? Category { get; set; } 
    }
}
