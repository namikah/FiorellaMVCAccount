using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FirstFiorellaMVC.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, StringLength(maximumLength:100)]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
