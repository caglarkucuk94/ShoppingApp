using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using ShoppingApp.Migrations;
using ShoppingApp.Models;
using ShoppingApp.Models.Dtos;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace ShoppingApp.Controllers
{
    public class AccountController : Controller
    {
        MyContext myContext = new MyContext();


        public IActionResult Register()
        {
            //if (!Request.Cookies.ContainsKey("mail"))
            //{
            //    return RedirectToAction("SignIn");
            //}
            return View();
        }
        [HttpPost]
        public IActionResult Register(UserDto userDto)
        {
            User user = new User();

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (myContext.User.Where(x => x.Email.Contains(userDto.Email)).Count() > 0)
            {
                ModelState.AddModelError("Email", "Bu mail adresi daha önceden mevcut");
                return View();
            }
            string pass = userDto.Password;

            Regex regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[$@$!%*?&+=#]) [A-Za-z\\d$@$!%*?&+=#]{8,16}$");
            Match match = regex.Match(pass);

            if (!match.Success)
            {
                ModelState.AddModelError("Password", "Şifre rakam ve büyük küçük harf içermelidir");
                return View();
            }

            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.Surname = userDto.Surname;
            user.Password = userDto.Password;

            myContext.User.Add(user);
            myContext.SaveChanges();


            return View();
        }


        public IActionResult SignIn()
        {



            return View();
        }
        [HttpPost]
        public IActionResult SignIn(UserSignInDto userSignInDto)
        {

            if (ModelState.IsValid)
            {

                User user = myContext.User.Where(x => x.Password == userSignInDto.Password && x.Email == userSignInDto.Email).FirstOrDefault();
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                if (user != null)
                {
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    if (user.IsAdmin)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                        Response.Cookies.Append("admin", user.IsAdmin.ToString());
                    }
                    else
                    {
                        Response.Cookies.Delete("admin");
                    }

                    identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
                    identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                    identity.AddClaim(new Claim(ClaimTypes.Surname, user.Surname));


                    var principal = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("List", "UserList");



                }
                else
                {
                    ModelState.AddModelError("Email", "Bilgilere uygun hesap bulunamamıştır.");
                    return View();

                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }



        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult LogOut()
        {
            Response.Cookies.Delete("mail");
            return RedirectToAction("SignIn", "Account");

        }
    }
}
