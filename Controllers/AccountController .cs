using AppMVC.DTOs;
using AppMVC.Helpers;
using AppMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppMVC.Controllers
{
    public class AccountController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;

        [HttpGet]
        public IActionResult Register() => View();
        [HttpPost]
        public IActionResult Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            var user = _userService.Register(dto);
            if (user == null)
            {
                ModelState.AddModelError("", "User already exists");
                return View(dto);
            }
            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public IActionResult Login() => View();

        //[RoleAuthorize("Admin")]
        [HttpPost]
        public IActionResult Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            var user = _userService.Login(dto);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(dto);
            }
            HttpContext.Session.SetString("Token", user.Token);
            var token = HttpContext.Session.GetString("Token");
               Console.WriteLine($"Token in session: {token}");
            return RedirectToAction("Index", "Product");
        }
        public IActionResult Logout()
        {
            // Clear session/cookie
            HttpContext.Session.Remove("Token");
            HttpContext.Session.Remove("cart");
            return RedirectToAction("Login");
        }

    }
}
