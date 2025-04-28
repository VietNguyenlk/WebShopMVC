using AppMVC.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AppMVC.Controllers.Admin
{
    [RoleAuthorize("Admin")]
    public class AdminController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
