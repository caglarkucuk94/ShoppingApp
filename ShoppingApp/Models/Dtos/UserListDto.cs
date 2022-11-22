using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Models.Dtos
{
    public class UserListDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int UserId { get; set; }
        public bool IsShop { get; set; }

    }
}
