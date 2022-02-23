using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FirstFiorellaMVC.Models
{
    public class Position
    {
        public int Id { get; set; } 

        [Required]
        public string Name { get; set; }

        public ICollection<Expert> Experts { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
