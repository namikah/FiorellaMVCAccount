using FirstFiorellaMVC.Areas.AdminPanel.Data;
using FirstFiorellaMVC.Data;
using FirstFiorellaMVC.DataAccessLayer;
using FirstFiorellaMVC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FirstFiorellaMVC.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SliderImageController : Controller
    {
        private readonly AppDbContext _dbContext;

        private readonly IWebHostEnvironment _environment;


        public SliderImageController(AppDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }


        public async Task<IActionResult> Index()
        {
            ViewBag.ImageMaxCount = SliderImageSetting.ImageMaxCount;
            var sliderImages = await _dbContext.SliderImages.OrderByDescending(x=>x.Id).ToListAsync();

            return View(sliderImages);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderImage sliderImage)
        {
            if (!ModelState.IsValid)
                return View();

            var sliderImageCount = await _dbContext.SliderImages.CountAsync();
            if(sliderImageCount + sliderImage.Photos.Length > SliderImageSetting.ImageMaxCount)
            {
                ModelState.AddModelError("Photos", $"You can only {SliderImageSetting.ImageMaxCount - sliderImageCount} images add");
                return View();
            }

            foreach (var photo in sliderImage.Photos)
            {
                if (!photo.IsImage())
                {
                    ModelState.AddModelError("Photos", $"{photo.FileName} - Yuklediyiniz shekil olmalidir.");
                    return View();
                }

                if (!photo.IsAllowedSize(1))
                {
                    ModelState.AddModelError("Photos", $"{photo.FileName} - shekil 1 mb-dan az olmalidir.");
                    return View();
                }

                var fileName = await photo.GenerateFile(Constants.ImageFolderPath);

                var newSliderImage = new SliderImage { Name = fileName };
                await _dbContext.SliderImages.AddAsync(newSliderImage);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var sliderImage = await _dbContext.SliderImages.FindAsync(id);
            if (sliderImage == null)
                return NotFound();

            return View(sliderImage);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SliderImage sliderImage)
        {
            if (id == null)
                return NotFound();

            if (id != sliderImage.Id)
                return BadRequest();

            var existSliderImage = await _dbContext.SliderImages.FindAsync(id);
            if (existSliderImage == null)
                return NotFound();

            if (!ModelState.IsValid)
                //return View(existSliderImage);
                return Json(sliderImage.Photos);

            if (!sliderImage.Photos[0].IsImage())
            {
                ModelState.AddModelError("Photo", "Yuklediyiniz shekil olmalidir.");
                return View(existSliderImage);
            }

            if (!sliderImage.Photos[0].IsAllowedSize(1))
            {
                ModelState.AddModelError("Photo", "1 mb-dan az olmalidir.");
                return View(existSliderImage);
            }

            var path = Path.Combine(Constants.ImageFolderPath, existSliderImage.Name);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            var fileName = await sliderImage.Photos[0].GenerateFile(Constants.ImageFolderPath);
            existSliderImage.Name = fileName;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var existSliderImage = await _dbContext.SliderImages.FindAsync(id);
            if (existSliderImage == null)
                return NotFound();

            var path = Path.Combine(Constants.ImageFolderPath, existSliderImage.Name);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            else
            {
                return Json(new { status = false });
            }

            _dbContext.SliderImages.Remove(existSliderImage);
            await _dbContext.SaveChangesAsync();

            return Json(new { status = true });
        }
    }
}
