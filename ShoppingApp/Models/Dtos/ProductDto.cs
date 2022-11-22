using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Models.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile File { get; set; }
        public string? FileName { get; set; }
        public string? FileBase64String { get; set; }
        public string? ContentType { get; set; }
        public int CategoryId { get; set; }
        public virtual CategoryDto? Category { get; set; }
    }
}
