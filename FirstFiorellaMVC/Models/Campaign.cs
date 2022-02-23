using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FirstFiorellaMVC.Models
{
    public class Campaign
    {
        public int Id { get; set; }

        [Required, StringLength(maximumLength: 100)]
        public string Name { get; set; }

        [Required]
        public int Discount { get; set; }

        public List<Product> Products { get; set; }
    }
}
