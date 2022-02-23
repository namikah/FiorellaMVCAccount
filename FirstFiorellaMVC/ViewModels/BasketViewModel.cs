using FirstFiorellaMVC.Models;
using System.Collections.Generic;

namespace FirstFiorellaMVC.ViewModels
{
    public class BasketViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string Dimension { get; set; }

        public string Weight { get; set; }

        public string SKUCode { get; set; }

        public int Count { get; set; } = 1;

        public string Image { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int? CampaignId { get; set; }

        public Campaign Campaign { get; set; }

    }
}
