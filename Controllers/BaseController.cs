using AppMVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppMVC.Controllers
{
    public class BaseController : Controller
    {
        protected readonly JwtService _jwtService;

        public BaseController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }
        // This method is used to get the logged-in user from the JWT token stored in the session

        protected ClaimsPrincipal? GetLoggedInUser()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token)) return null;

            return _jwtService.DecodeToken(token);
        }
    }
}
