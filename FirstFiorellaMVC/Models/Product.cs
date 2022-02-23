using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstFiorellaMVC.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }

        public string Dimension { get; set; }

        public string Weight { get; set; }

        public string SKUCode { get; set; }

        public  List<ProductImage> Images{ get; set; }

        [Required, ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int? CampaignId { get; set; }

        public Campaign Campaign { get; set; }
    }
}
