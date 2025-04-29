using AppMVC.Data;
using AppMVC.Helpers;
using AppMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppMVC.Controllers.Admin
{
    [RoleAuthorize("Admin")]
    public class OrderAdminController : Controller
    {
        private readonly AppDbContext _context;
        public OrderAdminController(AppDbContext context)
        {
            _context = context;
        }


        // hiển thị danh sách đơn hàng
        public async Task <IActionResult> Index()
        {
            var orders = await _context.Orders.Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ToListAsync();
            // return View("/Views/Admin/OrderAdmin/Index.cshtml");
           return View("/Views/Admin/OrderAdmin/Index.cshtml",orders);
        }
        // xem chi tiet đơn hàng
        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return View("/Views/Admin/OrderAdmin/Details.cshtml", order);
        }
    }
}
