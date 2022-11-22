using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Models.Dtos
{
    public class ProductUserListDto
    {
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int UserListId { get; set; }
        [Required]
        public int UserId { get; set; }
        public string? Description { get; set; }
        public bool IsBought { get; set; }
        public virtual ProductDto Product { get; set; }
    }
}
