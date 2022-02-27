using FirstFiorellaMVC.Data;
using FirstFiorellaMVC.DataAccessLayer;
using FirstFiorellaMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FirstFiorellaMVC.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = RoleConstants.AdminRole)]
    public class ProductController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ProductController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        public async Task<ActionResult> Index(int page = 1)
        {
            ViewBag.ProductCounts = await _dbContext.Products.CountAsync();
            ViewBag.CurrentPage = page;

            var products = await _dbContext.Products.Include(x=>x.Images).Include(x=>x.Category).Include(x => x.Campaign).OrderByDescending(x => x.Id).Skip((page - 1) * 4).Take(4).ToListAsync();

            return View(products);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Detail(int? id)
        {
            if (id == null)
                return BadRequest();

            var product =await _dbContext.Products.Include(x=>x.Images).Include(x=>x.Category).Include(x=>x.Campaign).Where(x=>x.Id == id).FirstOrDefaultAsync();
            if (product == null)
                return NotFound();

            return View(product);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var isProduct = await _dbContext.Products.AnyAsync(x => x.Name.ToLower() == product.Name.ToLower());
            if (isProduct)
            {
                ModelState.AddModelError("Name", "Already exist");
                return View();
            }

            var isCategory = await _dbContext.Categories.AnyAsync(x => x.Id == product.CategoryId);
            if (!isCategory)
            {
                ModelState.AddModelError("CategoryId", "Not found category");
                return View();
            }

            

            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            #region Add Image for product to Images list (muveqqeti)
            await _dbContext.ProductImages.AddAsync(new ProductImage()
            {
                Name = "shop-14-img.jpg",
                ProductId = product.Id,
                IsMain = true
            });
            await _dbContext.SaveChangesAsync();
            #endregion

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _dbContext.Products.Include(x=>x.Category).FirstOrDefaultAsync(x=>x.Id == id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var isExistProduct = await _dbContext.Products.FindAsync(id);
            if (isExistProduct == null)
            {
                ModelState.AddModelError("Name", "Not found");
                return View(isExistProduct);
            }

            var isExistProductName = await _dbContext.Products.Where(x=>x.Id != product.Id).AnyAsync(x => x.Name == product.Name);
            if (isExistProductName)
            {
                ModelState.AddModelError("Name", "Product name already exist");
                return View(isExistProduct);
            }

            isExistProduct.Name = product.Name;
            isExistProduct.Dimension = product.Dimension;
            isExistProduct.Weight = product.Weight;
            isExistProduct.SKUCode = product.SKUCode;
            isExistProduct.Price = product.Price;
            isExistProduct.CategoryId = product.CategoryId;
            isExistProduct.CampaignId = product.CampaignId;


            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<JsonResult> Delete(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
                return Json(new { status = false });

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();

            return Json(new { status = true });
        }
    }
}
