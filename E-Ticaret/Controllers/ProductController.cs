using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Mvc.Rendering; 
using Microsoft.EntityFrameworkCore; 
using ETicaret.Data; 
using ETicaret.Models; 

namespace ETicaret.Controllers
{
    public class ProductController : Controller // Ürün CRUD işlemleri için controller
    {
        private readonly AppDbContext _context; // Veritabanı bağlantısı

        public ProductController(AppDbContext context)
        {
            _context = context; // DI ile gelen DbContext'i atıyorum
        }

        public async Task<IActionResult> Index() // Ürünleri listele
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync(); // Kategori bilgisiyle birlikte getir
            return View(products); // Listeyi view'a gönderiyorum
        }

        public async Task<IActionResult> Details(int? id) // Ürün detayları
        {
            if (id == null)
                return NotFound(); // ID null ise 404 döndür

            var product = await _context.Products.Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id); // ID'ye göre ürünü bul

            if (product == null)
                return NotFound(); // Ürün yoksa 404

            return View(product); // Ürünü detay view'ına gönder
        }

        public IActionResult Create() // Ürün ekleme formu (GET)
        {
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name"); // Kategorileri viewBag'e yüklüyorum
            return View(); // Formu görüntüle
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product) // Ürün ekleme işlemi (POST)
        {
            if (ModelState.IsValid) // Doğrulama başarılıysa
            {
                _context.Products.Add(product); // Ürünü ekle
                await _context.SaveChangesAsync(); // Değişiklikleri kaydet
                return RedirectToAction(nameof(Index)); // Liste sayfasına yönlendir
            }
            // ModelState hatalarını Debug Output'a yazdırıyorum
            foreach (var key in ModelState.Keys)
            {
                foreach (var error in ModelState[key].Errors)
                {
                    System.Diagnostics.Debug.WriteLine($"{key}: {error.ErrorMessage}"); // Hata mesajını yazdır
                }
            }
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId); // Dropdown'ı yeniden yükle
            return View(product); // Formu hata mesajlarıyla birlikte yeniden göster
        }

        public async Task<IActionResult> Edit(int? id) // Ürün düzenleme formu (GET)
        {
            if (id == null)
                return NotFound(); // ID null ise 404

            var product = await _context.Products.FindAsync(id); // Ürünü bul
            if (product == null)
                return NotFound(); // Ürün bulunamazsa 404

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId); // Kategorileri yükle
            return View(product); // Formu ürün bilgileriyle gönder
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product) // Ürün güncelleme (POST)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Update(product); // Ürünü güncelle
                await _context.SaveChangesAsync(); // Kaydet
                return RedirectToAction(nameof(Index)); // Listeye yönlendir
            }
            foreach (var key in ModelState.Keys) // Hataları debug output'a yazdır
            {
                foreach (var error in ModelState[key].Errors)
                {
                    System.Diagnostics.Debug.WriteLine($"{key}: {error.ErrorMessage}");
                }
            }
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId); // Dropdown'ı tekrar yükle
            return View(product); // Formu hatalarla yeniden göster
        }

        public async Task<IActionResult> Delete(int? id) // Ürün silme formu (GET)
        {
            if (id == null)
                return NotFound(); // ID null ise 404

            var product = await _context.Products.Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id); // Ürünü kategori bilgisiyle bul
            if (product == null)
                return NotFound(); // Ürün yoksa 404

            return View(product); // Silme onay sayfasına gönder
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // Ürün silme işlemi (POST)
        {
            var product = await _context.Products.FindAsync(id); // Ürünü bul
            if (product != null)
            {
                _context.Products.Remove(product); // Ürünü sil
                await _context.SaveChangesAsync(); // Kaydet
            }
            return RedirectToAction(nameof(Index)); // Listeye yönlendir
        }
    }
}
