using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Models.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanını boş bırakamazsınız")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyad alanını boş bırakamazsınız")]

        public string Surname { get; set; }

        [Required(ErrorMessage = "Email alanını boş bırakamazsınız")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage = "Lütfen geçerli bir email adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanını boş bırakamazsınız")]
        [StringLength(50, ErrorMessage = "Şifre en az {2} karakter olmalı", MinimumLength = 8)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre alanını boş bırakamazsınız")]
        [Compare("Password", ErrorMessage = "Şifreler Aynı Değil")]
        public string PasswordConfirmation { get; set; }
        public bool IsAdmin { get; set; }
    }
}
