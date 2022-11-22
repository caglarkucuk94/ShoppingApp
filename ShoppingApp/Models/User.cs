using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingApp.Models
{
    [Table ("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        [Required]
        public string Email { get; set; }
        public string  Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}
