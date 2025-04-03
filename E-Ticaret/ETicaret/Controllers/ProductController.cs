using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Mvc.Rendering; 
using Microsoft.EntityFrameworkCore; 
using ETicaret.Data; 
using ETicaret.Models; 

namespace ETicaret.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context; // Veritabanı bağlantısı

        public ProductController(AppDbContext context)
        {
            _context = context; 
        }

        public async Task<IActionResult> Index() // Ürünleri listele
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync(); // Kategori bilgisiyle birlikte getir
            return View(products);
        }

        public async Task<IActionResult> Details(int? id) // Ürün detayları
        {
            if (id == null)
                return NotFound(); // ID null ise 404 döndür

            var product = await _context.Products.Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id); // ID'ye göre ürünü bul

            if (product == null)
                return NotFound(); // Ürün yoksa 404

            return View(product);
        }

        public IActionResult Create() 
        {
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name"); 
            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product) 
        {
            if (ModelState.IsValid) // Doğrulama başarılıysa
            {
                _context.Products.Add(product); // Ürünü ekle
                await _context.SaveChangesAsync(); // Değişiklikleri kaydet
                return RedirectToAction(nameof(Index)); // Liste sayfasına yönlendir
            }
            foreach (var key in ModelState.Keys)
            {
                foreach (var error in ModelState[key].Errors)
                {
                    System.Diagnostics.Debug.WriteLine($"{key}: {error.ErrorMessage}"); // Hata mesajını yazdır
                }
            }
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId); 
            return View(product); 
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product) 
        {
            if (ModelState.IsValid)
            {
                _context.Products.Update(product); // Ürünü güncelle
                await _context.SaveChangesAsync(); // Kaydet
                return RedirectToAction(nameof(Index)); // Listeye yönlendir
            }
            foreach (var key in ModelState.Keys) 
            {
                foreach (var error in ModelState[key].Errors)
                {
                    System.Diagnostics.Debug.WriteLine($"{key}: {error.ErrorMessage}");
                }
            }
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId); 
            return View(product); 
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound(); 

            var product = await _context.Products.Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id); // Ürünü kategori bilgisiyle bul
            if (product == null)
                return NotFound(); // Ürün yoksa 404

            return View(product); // Silme onay sayfasına gönder
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) 
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
