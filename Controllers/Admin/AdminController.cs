using AppMVC.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppMVC.Controllers.Admin
{
    [RoleAuthorize("Admin")]
    public class AdminController : Controller
    {

        public IActionResult Index()

        {
            var user = HttpContext.User; // JWT Middleware đã xử lý rồi
            if (user?.Identity?.Name != null) // Ensure user and Identity are not null
            {
                ViewBag.Username = user.Identity.Name;
                ViewBag.Role = user.FindFirst(ClaimTypes.Role)?.Value;
            }


            return View();
        }
    }
}
