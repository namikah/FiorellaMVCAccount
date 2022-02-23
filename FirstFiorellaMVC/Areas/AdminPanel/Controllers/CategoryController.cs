using FirstFiorellaMVC.DataAccessLayer;
using FirstFiorellaMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FirstFiorellaMVC.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _dbContext;

        public CategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ActionResult> Index(int page = 1)
        {
            ViewBag.CategoryCounts = await _dbContext.Categories.CountAsync();
            ViewBag.CurrentPage = page;

            var categories = await _dbContext.Categories.OrderByDescending(x => x.Id).Skip((page - 1) * 4).Take(4).ToListAsync();

            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var isCategory = await _dbContext.Categories.AnyAsync(x => x.Name.ToLower() == category.Name.ToLower());
            if (isCategory)
            {
                ModelState.AddModelError("Name", "Category already exist");
                return View();
            }

            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var isExistCategory = await _dbContext.Categories.FindAsync(category.Id);
            if (isExistCategory == null)
            {
                ModelState.AddModelError("Name", " Category not found");
                return View(isExistCategory);
            }

            var isExistCategoryName = await _dbContext.Categories.AnyAsync(x => x.Name == category.Name);
            if (isExistCategoryName)
            {
                ModelState.AddModelError("Name", "Category name already exist");
                return View(isExistCategory);
            }

            isExistCategory.Name = category.Name;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

       
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category == null)
                return Json(new { status = 404 });

            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
            return Json(new { status = 200 });
        }
    }
}
