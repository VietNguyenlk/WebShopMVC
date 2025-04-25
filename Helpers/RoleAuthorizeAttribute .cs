using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppMVC.Helpers
{
    public class RoleAuthorizeAttribute :Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;
        public RoleAuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            var userRole = user.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == null || !_roles.Contains(userRole))
            {
                context.Result = new ForbidResult(); // Hoặc Redirect đến trang lỗi
            }
        }
    }
   
}
