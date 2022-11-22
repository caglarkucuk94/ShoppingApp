using ShoppingApp.Models.Dtos;

namespace ShoppingApp.Models.ViewModel
{
    public class ProductUserListVM
    {
        public List<ProductUserList> ProductUserLists { get; set; }
        public List<ProductDto> Products { get; set; }
        public ProductUserList ProductUserList { get; set; }
        public UserList UserList { get; set; }
    }
}
