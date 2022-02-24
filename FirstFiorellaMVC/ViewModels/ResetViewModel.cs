using System.ComponentModel.DataAnnotations;

namespace FirstFiorellaMVC.ViewModels
{
    public class ResetViewModel
    {
        [Required]
        public string Username { get; set; }
    }
}
