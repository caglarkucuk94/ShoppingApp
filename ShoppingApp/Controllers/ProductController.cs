using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Models.Dtos;
using ShoppingApp.Models;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;

namespace ShoppingApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        MyContext myContext = new MyContext();
        private readonly IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Add()
        {
            List<Category> categories = myContext.Category.ToList();
            ViewBag.Category = new SelectList(categories.Select(x => new { x.Id, x.Name }), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Add(ProductDto productDto)
        {
            Product product = new Product();

            List<Category> categories = myContext.Category.ToList();
            ViewBag.Category = new SelectList(categories.Select(x => new { x.Id, x.Name }), "Id", "Name");

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (myContext.Product.Where(x => x.Name.Contains(productDto.Name)).Count() > 0)
            {
                ModelState.AddModelError("Name", "Bu ürün daha önceden mevcut");
                return View();
            }

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.FileName = productDto.File.FileName;
            product.ContentType = productDto.File.ContentType;
            product.CategoryId = productDto.CategoryId;
            using (var ms = new MemoryStream())
            {
                productDto.File.CopyTo(ms);
                var fileBytes = ms.ToArray();
                product.FileBase64String = Convert.ToBase64String(fileBytes);
            }

            myContext.Product.Add(product);
            myContext.SaveChanges();


            _memoryCache.Remove("Products");
            return RedirectToAction("List");
        }

        public IActionResult List()
        {
            List<ProductDto> products = new List<ProductDto>();

            products = myContext.Product.Include(k => k.Category).Select(x => new ProductDto
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

            ////var a = Convert.FromBase64String(products[0].FileBase64String.ToString());
            //ViewBag.Resim = "data:image/jpeg;base64," + products[0].FileBase64String.ToString();
            return View(products);
        }

        public IActionResult DeleteProduct(int id)
        {
            Product product = new Product();

            product = myContext.Product.Where(x => x.Id == id).FirstOrDefault();

            myContext.Product.Remove(product);
            myContext.SaveChanges();

            _memoryCache.Remove("Products");
            return RedirectToAction("List");
        }


        public IActionResult Edit(int id)
        {
            List<Category> categories = myContext.Category.ToList();
            ViewBag.Category = new SelectList(categories.Select(x => new { x.Id, x.Name }), "Id", "Name");

            var result = myContext.Product.Include(k => k.Category).Where(x => x.Id == id).Select(x => new ProductDto
            {
                Name = x.Name,
                Id = x.Id,
                Description = x.Description,
                FileBase64String = String.Format("data:{0};base64,{1}", x.ContentType, x.FileBase64String.ToString()),
                FileName = x.FileName,
                CategoryId = x.CategoryId,
                Category = new CategoryDto()
                {
                    Id = x.CategoryId,
                    Name = x.Category.Name
                }
            }).FirstOrDefault();

            return View(result);
        }


        [HttpPost]

        public IActionResult Edit(ProductDto productDto)
        {

            Product product = new Product();

            List<Category> categories = myContext.Category.ToList();
            ViewBag.Category = new SelectList(categories.Select(x => new { x.Id, x.Name }), "Id", "Name");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit");
            }

            if (myContext.Product.Where(x => x.Name.Contains(productDto.Name) && x.Id != productDto.Id).Count() > 0)
            {
                ModelState.AddModelError("Name", "Bu ürün daha önceden mevcut");
                return View();
            }

            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.FileName = productDto.File.FileName;
            product.ContentType = productDto.File.ContentType;
            product.CategoryId = productDto.CategoryId;
            product.Id = productDto.Id;
            using (var ms = new MemoryStream())
            {
                productDto.File.CopyTo(ms);
                var fileBytes = ms.ToArray();
                product.FileBase64String = Convert.ToBase64String(fileBytes);
            }

            myContext.Product.Update(product);
            myContext.SaveChanges();


            _memoryCache.Remove("Products");
            return RedirectToAction("List");
        }

    }
}
