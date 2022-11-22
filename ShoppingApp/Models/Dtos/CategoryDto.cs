using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Models.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı boş bırakamazsınız")]
        public string Name { get; set; }
    }
}
