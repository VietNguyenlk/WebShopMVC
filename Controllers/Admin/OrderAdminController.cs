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
        // tìm kiếm hóa đơn
        public async Task<IActionResult> Search(string searchString)
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => (o.User != null && o.User.Name.Contains(searchString)) || o.Id.ToString().Contains(searchString))
                .ToListAsync();
            return View("/Views/Admin/OrderAdmin/Search.cshtml", orders);
        }
        // xóa đơn hàng
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // cập nhật trạng thái đơn hàng
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            order.Status = status;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
