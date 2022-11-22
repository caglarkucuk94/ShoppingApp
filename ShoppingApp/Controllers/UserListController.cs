using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Models.Dtos;
using ShoppingApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingApp.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;

namespace ShoppingApp.Controllers
{
    [Authorize]
    public class UserListController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public UserListController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        MyContext myContext = new MyContext();

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(UserListDto userListDto)
        {
            UserList userList = new UserList();
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (myContext.UserList.Where(x => x.Name.Contains(userListDto.Name) && x.UserId == userId).Count() > 0)
            {
                ModelState.AddModelError("Name", "Bu listeden daha önceden mevcut");
                return View();
            }

            userList.Name = userListDto.Name;
            userList.UserId = userId;

            myContext.UserList.Add(userList);
            myContext.SaveChanges();

            return RedirectToAction("List");
        }

        public IActionResult List()
        {
            List<UserListDto> userLists = new List<UserListDto>();
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            userLists = myContext.UserList.Where(x => x.UserId == userId).Select(x => new UserListDto
            {
                Name = x.Name,
                Id = x.Id
            }).ToList();

            return View(userLists);
        }

        public IActionResult DeleteUserList(int id)
        {
            UserList userList = new UserList();

            userList = myContext.UserList.Where(x => x.Id == id).FirstOrDefault();

            myContext.UserList.Remove(userList);

            List<ProductUserList> productUserLists = myContext.ProductUserList.Where(x => x.UserListId == id).ToList();

            myContext.ProductUserList.RemoveRange(productUserLists);

            myContext.SaveChanges();

            return RedirectToAction("List");
        }

        public IActionResult UserListDetail(int id)
        {
            List<Category> categories = myContext.Category.ToList();
            ViewBag.Category = new SelectList(categories.Select(x => new { x.Id, x.Name }), "Id", "Name");

            ProductUserListVM productUserListVM = new ProductUserListVM();
            ProductUserList productUserList = new ProductUserList();
            productUserList.UserListId = id;
            productUserListVM.ProductUserList = productUserList;

            productUserListVM.UserList = myContext.UserList.Where(x => x.Id == id).FirstOrDefault();

            productUserListVM.ProductUserLists = myContext.ProductUserList.Where(x => x.UserListId == id).Include(x => x.Product).ToList();

            if (_memoryCache.TryGetValue("Products", out List<ProductDto> products))
            {
                productUserListVM.Products = products;
            }
            else
            {
                var result = myContext.Product.Select(x => new ProductDto
                {
                    Name = x.Name,
                    Id = x.Id,
                    Description = x.Description,
                    FileBase64String = String.Format("data:{0};base64,{1}", x.ContentType, x.FileBase64String.ToString()),
                    FileName = x.FileName,
                    Category = new CategoryDto()
                    {
                        Id = x.CategoryId,
                        Name = x.Category.Name
                    }
                }).ToList();

                _memoryCache.Set("Products", result, TimeSpan.FromMinutes(5));

                productUserListVM.Products = result;
            }



            return View(productUserListVM);
        }

        [HttpPost]
        public IActionResult UserListDetail(ProductUserListVM productUserListVM)
        {
            List<Category> categories = myContext.Category.ToList();
            ViewBag.Category = new SelectList(categories.Select(x => new { x.Id, x.Name }), "Id", "Name");

            if (myContext.ProductUserList.Where(x => x.UserListId == productUserListVM.ProductUserList.UserListId && x.ProductId == productUserListVM.ProductUserList.ProductId).Count() > 0)
            {
                return RedirectToAction("UserListDetail");
            }

            ProductUserList productUserList = new ProductUserList();
            productUserList.UserListId = productUserListVM.ProductUserList.UserListId;
            productUserList.Description = productUserListVM.ProductUserList.Description;
            productUserList.ProductId = productUserListVM.ProductUserList.ProductId;
            productUserList.UserId = (int)HttpContext.Session.GetInt32("UserId");

            myContext.ProductUserList.Add(productUserList);
            myContext.SaveChanges();

            return RedirectToAction("UserListDetail");
        }

        public IActionResult ProductListRemoveProduct(int id)
        {
            ProductUserList productUserList = myContext.ProductUserList.Where(x => x.Id == id).FirstOrDefault();
            myContext.ProductUserList.Remove(productUserList);
            myContext.SaveChanges();
            ReturnModel returnModel = new ReturnModel();

            returnModel.id = productUserList.UserListId;

            return RedirectToAction("UserListDetail", returnModel);
        }

        public IActionResult ProductListEdit(int id)
        {
            ProductUserList productUserList = myContext.ProductUserList.Where(x => x.Id == id).Include(x => x.Product).FirstOrDefault();


            return View(productUserList);

        }
        [HttpPost]
        public IActionResult ProductListEdit(ProductUserList productUserList)
        {

            ProductUserList lastproductUserList = myContext.ProductUserList.Where(x => x.Id == productUserList.Id).FirstOrDefault();
            lastproductUserList.Description = productUserList.Description;

            myContext.ProductUserList.Update(lastproductUserList);
            myContext.SaveChanges();


            ReturnModel returnModel = new ReturnModel();
            returnModel.id = lastproductUserList.UserListId;
            return RedirectToAction("UserListDetail", returnModel);

        }

        public IActionResult IsShop(int id)
        {
            UserList userList = myContext.UserList.Where(x => x.Id == id).FirstOrDefault();
            userList.IsShop = true;


            myContext.UserList.Update(userList);
            myContext.SaveChanges();

            ReturnModel returnModel = new ReturnModel();
            returnModel.id = userList.Id;
            return RedirectToAction("UserListDetail", returnModel);
        }

        public IActionResult IsComplete(int id)
        {
            UserList userList = myContext.UserList.Where(x => x.Id == id).FirstOrDefault();
            userList.IsShop = false;


            myContext.UserList.Update(userList);

            List<ProductUserList> productUserLists = myContext.ProductUserList.Where(x => x.UserListId == id && x.IsBought == true).ToList();

            myContext.ProductUserList.RemoveRange(productUserLists);

            myContext.SaveChanges();

            ReturnModel returnModel = new ReturnModel();
            returnModel.id = userList.Id;
            return RedirectToAction("UserListDetail", returnModel);
        }

        public IActionResult IsBought(int id)
        {
            ProductUserList productUserList = myContext.ProductUserList.Where(x => x.Id == id).FirstOrDefault();
            productUserList.IsBought = true;


            myContext.ProductUserList.Update(productUserList);
            myContext.SaveChanges();

            ReturnModel returnModel = new ReturnModel();
            returnModel.id = productUserList.UserListId;
            return RedirectToAction("UserListDetail", returnModel);
        }


        public class ReturnModel
        { public int id { get; set; } }
    }
}
