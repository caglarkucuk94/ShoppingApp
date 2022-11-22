using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Models
{
    public class ProductUserList
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }
        public int UserListId { get; set; }
        public int UserId { get; set; }
        public string? Description { get; set; }
        public bool IsBought { get; set; }
        public virtual Product Product { get; set; }

    }
}
