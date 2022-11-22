using ShoppingApp.Models.Dtos;
using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string FileName { get; set; }
        public string FileBase64String { get; set; }
        public string ContentType { get; set; }
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }

    }
}
