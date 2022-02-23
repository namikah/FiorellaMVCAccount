using FirstFiorellaMVC.DataAccessLayer;
using FirstFiorellaMVC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FirstFiorellaMVC.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _dbContext;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ActionResult> Index(int page = 1)
        {
            ViewBag.BlogCounts = await _dbContext.Blogs.CountAsync();
            ViewBag.CurrentPage = page;

            var blogs = await _dbContext.Blogs.OrderByDescending(x => x.Id).Skip((page - 1) * 4).Take(4).ToListAsync();

            return View(blogs);
        }

        public async Task<ActionResult> Detail(int? id)
        {
            if (id == null)
                return BadRequest();

            var blog = await _dbContext.Blogs.FindAsync(id);
            if (blog == null)
                return NotFound();

            return View(blog);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            if (!ModelState.IsValid)
            {
                //return View();
            }

            #region Upload Image, Validation

            var isImageType = blog.Photo.ContentType.Contains("image");
            if (!isImageType)
            {
                ModelState.AddModelError("Photo", "uploaded file must be an image");
                return View();
            }

            var isImageSize = blog.Photo.Length;
            if (isImageSize > 1024 * 1000)
            {
                ModelState.AddModelError("Photo", "uploaded file must be max 1MB");
                return View();
            }

            var webRootPath = _webHostEnvironment.WebRootPath;

            var fileName = $"{Guid.NewGuid()}-{blog.Photo.FileName}";

            var path = Path.Combine(webRootPath, "img", fileName);

            var fileStream = new FileStream(path, FileMode.CreateNew);

            await blog.Photo.CopyToAsync(fileStream);

            #endregion

            var isBlogName = await _dbContext.Blogs.AnyAsync(x => x.Name.ToLower() == blog.Name.ToLower());
            if (isBlogName)
            {
                ModelState.AddModelError("Name", "Already exist this blog");
                return View();
            }

            blog.Image = fileName;

            await _dbContext.Blogs.AddAsync(blog);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var blog = await _dbContext.Blogs.FindAsync(id);
            if (blog == null)
                return NotFound();

            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            #region Upload Image, Validation

            var isImageType = blog.Photo.ContentType.Contains("image");
            if (!isImageType)
            {
                ModelState.AddModelError("Photo", "uploaded file must be an image");
                return View();
            }

            var isImageSize = blog.Photo.Length;
            if (isImageSize > 1024 * 1000)
            {
                ModelState.AddModelError("Photo", "uploaded file must be max 1MB");
                return View();
            }

            var webRootPath = _webHostEnvironment.WebRootPath;

            var fileName = $"{Guid.NewGuid()}-{blog.Photo.FileName}";

            var path = Path.Combine(webRootPath, "img", fileName);

            var fileStream = new FileStream(path, FileMode.CreateNew);

            await blog.Photo.CopyToAsync(fileStream);

            #endregion

            var isExistBlog = await _dbContext.Blogs.FindAsync(id);
            if (isExistBlog == null)
            {
                ModelState.AddModelError("Name", "Not found");
                return View(isExistBlog);
            }

            var isExistBlogName = await _dbContext.Blogs.Where(x=>x.Id != id).AnyAsync(x => x.Name == blog.Name);
            if (isExistBlogName)
            {
                ModelState.AddModelError("Name", "Blog name already exist");
                return View(isExistBlog);
            }
            isExistBlog.Name = blog.Name;
            isExistBlog.Description = blog.Description;
            isExistBlog.Datetime = blog.Datetime;
            isExistBlog.Context = blog.Context;
            isExistBlog.Image = fileName;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

       
        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _dbContext.Blogs.FindAsync(id);
            if (blog == null)
                return Json(new { status = 404 });

            _dbContext.Blogs.Remove(blog);
            _dbContext.SaveChanges();
            return Json(new { status = 200 });
        }
    }
}
