using System.ComponentModel.DataAnnotations;

namespace FirstFiorellaMVC.ViewModels
{
    public class RegisterViewModel : PasswordViewModel
    {
        [Required]
        public string Fullname { get; set; }

        [Required]
        public string Username { get; set; }

        [Required, EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
