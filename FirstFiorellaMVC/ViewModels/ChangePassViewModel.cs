using System.ComponentModel.DataAnnotations;

namespace FirstFiorellaMVC.ViewModels
{
    public class ChangePassViewModel
    {
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }


        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
