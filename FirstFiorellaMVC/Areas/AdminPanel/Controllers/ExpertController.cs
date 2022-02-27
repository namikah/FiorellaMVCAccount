using FirstFiorellaMVC.Data;
using FirstFiorellaMVC.DataAccessLayer;
using FirstFiorellaMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FirstFiorellaMVC.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = RoleConstants.AdminRole)]
    public class ExpertController : Controller
    {
        private readonly AppDbContext _dbContext;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExpertController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [AllowAnonymous]
        public async Task<ActionResult> Index(int page = 1)
        {
            ViewBag.Counts = await _dbContext.Experts.CountAsync();
            ViewBag.CurrentPage = page;

            var experts = await _dbContext.Experts.Include(x => x.Position).OrderByDescending(x => x.Id).Skip((page - 1) * 4).Take(4).ToListAsync();

            return View(experts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expert expert)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var isPosition = await _dbContext.Positions.AnyAsync(x => x.Id == expert.PositionId);
            if (!isPosition)
            {
                ModelState.AddModelError("PositionId", "Not found Position");
                return View();
            }

            #region Upload Image, Validation

            var isImageType = expert.Photo.ContentType.Contains("image");
            if (!isImageType)
            {
                ModelState.AddModelError("Photo", "uploaded file must be an image");
                return View();
            }

            var isImageSize = expert.Photo.Length;
            if (isImageSize > 1024 * 1000)
            {
                ModelState.AddModelError("Photo", "uploaded file must be max 1MB");
                return View();
            }

            var webRootPath = _webHostEnvironment.WebRootPath;

            var fileName = $"{Guid.NewGuid()}-{expert.Photo.FileName}";

            var path = Path.Combine(webRootPath, "img", fileName);

            var fileStream = new FileStream(path, FileMode.CreateNew);

            await expert.Photo.CopyToAsync(fileStream);

            #endregion

            expert.Image = fileName;

            await _dbContext.Experts.AddAsync(expert);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var expert = await _dbContext.Experts.Include(x => x.Position).FirstOrDefaultAsync(x => x.Id == id);
            if (expert == null)
                return NotFound();

            return View(expert);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Expert expert)
        {
            var isExistExpert = await _dbContext.Experts.FindAsync(id);

            if (!ModelState.IsValid)
            {
                return View(isExistExpert);
            }

            if (isExistExpert == null)
            {
                ModelState.AddModelError("Name", "Not found");
                return View(isExistExpert);
            }

            if(expert.Photo != null)
            {
            #region Upload Image, Validation
            var isImageNull = expert.Photo;
            if (isImageNull == null)
            {
                ModelState.AddModelError("Photo", "nothing found");
                return View(isExistExpert);
            }

            var isImageType = expert.Photo.ContentType.Contains("image");
            if (!isImageType)
            {
                ModelState.AddModelError("Photo", "uploaded file must be an image");
                return View(isExistExpert);
            }

            var isImageSize = expert.Photo.Length;
            if (isImageSize > 1024 * 1000)
            {
                ModelState.AddModelError("Photo", "uploaded file must be max 1MB");
                return View(isExistExpert);
            }

            var webRootPath = _webHostEnvironment.WebRootPath;

            var fileName = $"{Guid.NewGuid()}-{expert.Photo.FileName}";

            var path = Path.Combine(webRootPath, "img", fileName);

            var fileStream = new FileStream(path, FileMode.CreateNew);

            await expert.Photo.CopyToAsync(fileStream);

            isExistExpert.Image = fileName;

            #endregion
            }

            isExistExpert.Name = expert.Name;
            isExistExpert.PositionId = expert.PositionId;


            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<JsonResult> Delete(int id)
        {
            var expert = await _dbContext.Experts.FindAsync(id);
            if (expert == null)
                return Json(new { status = false });

            _dbContext.Experts.Remove(expert);
            _dbContext.SaveChanges();

            return Json(new { status = true });
        }
    }
}
