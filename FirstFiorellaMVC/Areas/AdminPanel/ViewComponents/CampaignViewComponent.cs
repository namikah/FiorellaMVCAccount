using FirstFiorellaMVC.DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FirstFiorellaMVC.ViewComponents
{
    public class CampaignViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public CampaignViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? id)
        {
            ViewBag.SelectedCampaign = id;
            var campaigns = await _dbContext.Campaigns.ToListAsync();

            return View(campaigns);
        }
    }
}
