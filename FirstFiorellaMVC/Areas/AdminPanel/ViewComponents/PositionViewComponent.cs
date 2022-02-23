using FirstFiorellaMVC.DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FirstFiorellaMVC.ViewComponents
{
    public class PositionViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public PositionViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? id)
        {
            ViewBag.SelectedPosition = id;
            var positions = await _dbContext.Positions.ToListAsync();

            return View(positions);
        }
    }
}
