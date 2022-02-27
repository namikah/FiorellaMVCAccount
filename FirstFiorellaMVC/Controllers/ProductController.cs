using FirstFiorellaMVC.DataAccessLayer;
using FirstFiorellaMVC.Models;
using FirstFiorellaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FirstFiorellaMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly int _productsCount;

        public ProductController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _productsCount = _appDbContext.Products.Count();
        }

        public  async Task<IActionResult> Index()
        {
            ViewBag.ProductsCount = _productsCount;
            var products = await _appDbContext.Products.Include(x => x.Category).Include(x=>x.Images).Take(8).ToListAsync();

            return View(products);

        }

        public async Task<IActionResult> Load(int skip)
        { 
            if (skip >= _productsCount)
            {
                return RedirectToAction(nameof(Index), "Error", BadRequest());
            }

            var products = await _appDbContext.Products.Include(x => x.Category).Include(x=>x.Images).Skip(skip).Take(8).ToListAsync();

            return PartialView("_ProductPartial", products);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            return View(new HomeViewModel
            {
                Products = await _appDbContext.Products.Include(p=>p.Campaign).ToListAsync(),
                ProductImages = await _appDbContext.ProductImages.ToListAsync(),
                ImagesByProductId = await _appDbContext.ProductImages.Where(x => x.Product.Id == id).ToListAsync(),
            });
        }


    }
}
