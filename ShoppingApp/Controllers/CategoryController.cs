using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Models;
using ShoppingApp.Models.Dtos;

namespace ShoppingApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        MyContext myContext = new MyContext();
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(CategoryDto categoryDto)
        {
            Category category = new Category();

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (myContext.Category.Where(x => x.Name.Contains(categoryDto.Name)).Count() > 0)
            {
                ModelState.AddModelError("Name", "Bu kategori daha önceden mevcut");
                return View();
            }

            category.Name = categoryDto.Name;

            myContext.Category.Add(category);
            myContext.SaveChanges();

            return RedirectToAction("List");
        }

        public IActionResult List()
        {
            List<CategoryDto> categories = new List<CategoryDto>();

            categories = myContext.Category.Select(x => new CategoryDto
            {
                Name = x.Name,
                Id = x.Id
            }).ToList();

            return View(categories);
        }

        public IActionResult DeleteCategory(int id)
        {
            Category category = new Category();

            category = myContext.Category.Where(x => x.Id == id).FirstOrDefault();

            myContext.Category.Remove(category);
            myContext.SaveChanges();

            return RedirectToAction("List");
        }
        public IActionResult Edit(int id)
        {
            var result = myContext.Category.Where(x => x.Id == id).FirstOrDefault();

            CategoryDto categoryDto = new CategoryDto();
            categoryDto.Id = result.Id;
            categoryDto.Name = result.Name;

            return View(categoryDto);
        }


        [HttpPost]

        public IActionResult Edit(CategoryDto categoryDto)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            Category lastCategory = myContext.Category.Where(x => x.Id == categoryDto.Id).FirstOrDefault();
            lastCategory.Name = categoryDto.Name;

            myContext.Category.Update(lastCategory);
            myContext.SaveChanges();

            return RedirectToAction("List");
        }
    }
}
