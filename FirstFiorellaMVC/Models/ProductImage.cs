using System.ComponentModel.DataAnnotations;

namespace FirstFiorellaMVC.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength:50)]
        public string Name { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public bool IsMain { get; set; }
    }
}
