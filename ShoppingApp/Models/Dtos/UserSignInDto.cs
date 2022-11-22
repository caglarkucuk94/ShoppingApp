using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Models.Dtos
{
    public class UserSignInDto
    {

        [Required(ErrorMessage = "Email alanını boş bırakamazsınız")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanını boş bırakamazsınız")]
        public string Password { get; set; }

    }
}
