using System.Collections.Generic; 
using System.ComponentModel.DataAnnotations;

namespace ETicaret.Models
{
    public class Category 
    {
        public int CategoryId { get; set; } // Otomatik artan kategori ID'si
        [Required(ErrorMessage = "Kategori adı zorunludur.")] public string Name { get; set; } // Kategori adı, zorunlu alan
        public ICollection<Product>? Products { get; set; } // Bu kategoriye ait ürünler
    }
}
