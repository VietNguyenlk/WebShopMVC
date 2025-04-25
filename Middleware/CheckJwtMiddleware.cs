using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AppMVC.Services;

namespace AppMVC.Middleware
{
    public class CheckJwtMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckJwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

      public async Task Invoke(HttpContext context, JwtService jwtService)
{
            //var token = HttpContext.Session.GetString("Token");
              var token = context.Session.GetString("Token");
            Console.WriteLine($"Token from session: {token ?? "null"}");
    
    if (!string.IsNullOrEmpty(token))
    {
        var user = jwtService.DecodeToken(token);
        Console.WriteLine($"User after decode: {user?.Identity?.Name ?? "null"}");
        
        if (user != null)
        {
            context.User = user;
        }
    }
    
    await _next(context);
}
    }
}
