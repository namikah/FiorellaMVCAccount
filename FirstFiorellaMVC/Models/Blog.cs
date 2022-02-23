using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace FirstFiorellaMVC.Models
{
    public class Blog
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Name Cannot be empty"), MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Context Cannot be empty"), MaxLength(150)]
        public string Context { get; set; }

        [Required(ErrorMessage = "Image Cannot be empty")]
        public string Image { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Please upload image.")]
        public IFormFile Photo { get; set; }

        public DateTime Datetime { get; set; }

        [StringLength(maximumLength:1000)]
        public string Description { get; set; }
    }
}
