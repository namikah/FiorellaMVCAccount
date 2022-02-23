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
    public class CampaignController : Controller
    {
        private readonly AppDbContext _dbContext;

        public CampaignController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ActionResult> Index(int page = 1)
        {
            ViewBag.CampaignCounts = await _dbContext.Campaigns.CountAsync();
            ViewBag.CurrentPage = page;

            var campaigns = await _dbContext.Campaigns.OrderByDescending(x => x.Id).Skip((page - 1) * 4).Take(4).ToListAsync();

            return View(campaigns);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Campaign campaign)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var isCampaign = await _dbContext.Campaigns.AnyAsync(x => x.Name.ToLower() == campaign.Name.ToLower());
            if (isCampaign)
            {
                ModelState.AddModelError("Name", "Campaign already exist");
                return View();
            }

            await _dbContext.Campaigns.AddAsync(campaign);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var campaign = await _dbContext.Campaigns.FindAsync(id);
            if (campaign == null)
                return NotFound();

            return View(campaign);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Campaign campaign)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var isExistCampaign = await _dbContext.Campaigns.FindAsync(campaign.Id);
            if (isExistCampaign == null)
            {
                ModelState.AddModelError("Name", " Campaign not found");
                return View(isExistCampaign);
            }

            var isExistCampaignName = await _dbContext.Campaigns.Where(x=>x.Id != campaign.Id).AnyAsync(x => x.Name == campaign.Name);
            if (isExistCampaignName)
            {
                ModelState.AddModelError("Name", "Campaign name already exist");
                return View(isExistCampaign);
            }

            isExistCampaign.Name = campaign.Name;
            isExistCampaign.Discount = campaign.Discount;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

       
        public async Task<IActionResult> Delete(int id)
        {
            var campaign = await _dbContext.Campaigns.FindAsync(id);
            if (campaign == null)
                return Json(new { status = 404 });

            _dbContext.Campaigns.Remove(campaign);
            _dbContext.SaveChanges();

            return Json(new { status = 200 });
        }
    }
}
