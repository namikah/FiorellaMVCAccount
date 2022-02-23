using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstFiorellaMVC.Models
{
    public class SliderImage
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Please select a file")]
        public IFormFile[] Photos { get; set; }
    }
}
