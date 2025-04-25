using AppMVC.Data;
using AppMVC.Models;
using AppMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AppMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var user = HttpContext.User; // JWT Middleware đã xử lý rồi
            if (user?.Identity?.Name != null) // Ensure user and Identity are not null
            {
                ViewBag.Username = user.Identity.Name;
                ViewBag.Role = user.FindFirst(ClaimTypes.Role)?.Value;
            }

            var products = _context.Products.ToList();
            return View(products);
        }
    }
}
