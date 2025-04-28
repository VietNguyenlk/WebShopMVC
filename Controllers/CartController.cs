    using AppMVC.Data;
using AppMVC.Helpers;
using AppMVC.Models;
using AppMVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace AppMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
       
        public CartController(AppDbContext context)
        {
            _context = context;
        
        }
    
        public IActionResult Index()
        {
            var cart = GetCartItems();
            return View(cart);
        }

        public IActionResult AddToCart(int productId)
        {

            // Logic to add the product to the cart
            // For example, you might use a service to handle cart operations
            // cartService.AddToCart(productId);
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                // Redirect to login page or show an error message
                return RedirectToAction("Login", "Account");
            }
            // Assuming you have a method to add the product to the cart
            var product = _context.Products.Find(productId);
            if (product == null)
            {
                // Handle the case where the product is not found
                return NotFound();
            }
            var cart =  GetCartItems();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem != null)
            {
                // If the item is already in the cart, increase the quantity
                cartItem.Quantity++;
            }
            else
            {
                // If the item is not in the cart, add it
                cart.Add(new CartItem
                {
                    ProductId = productId,
                    ProductName = product.Name,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Quantity = 1
                });
            }
            // Save the updated cart to session
            SaveCartToSession(cart);
            //TempData["Success"] = $"{product.Name} đã được thêm vào giỏ hàng!";


            // Trở về trang trước
            //return Redirect(Request.Headers["Referer"].ToString());

            return Json(new { success = true, message = $"{product.Name} đã được thêm vào giỏ hàng!" });
        }
        public List<CartItem> GetCartItems()
        {
            // Logic to retrieve cart items
            // For example, you might use a service to get the cart items
            // return cartService.GetCartItems();
            var sessionCart = HttpContext.Session.GetString("cart");
            return sessionCart != null
            ? JsonSerializer.Deserialize<List<CartItem>>(sessionCart) ?? new List<CartItem>()
            : new List<CartItem>();
        }
        private void SaveCartToSession(List<CartItem> cartItems)
        {
            var sessionCart = JsonSerializer.Serialize(cartItems);
            HttpContext.Session.SetString("cart", sessionCart);
        }
        // remove cart
        public IActionResult RemoveFromCart(int productId)
        {
            // remove cart from session
            var cart = GetCartItems();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
                SaveCartToSession(cart);
            }
            return RedirectToAction("Index");
        }
        // checkout
        [HttpPost]
        public IActionResult Checkout()
        {
            var cart = GetCartItems();
          //  var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
             // seesion luu cart vs token
            
         
            if (cart.Count == 0)
            {
                // Handle the case where the cart is empty
                return RedirectToAction("Index");
            }
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userID != null) {
                var order = new Order
                {
                    UserId = Convert.ToInt32(userID),
                    OverDateTime = DateTime.Now,
                    OrderDetails = cart.Select(item => new OrderDetail
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.Price
                    }).ToList()
                };
                _context.Orders.Add(order);
                _context.SaveChanges();
            }
            // Clear the cart after checkout
            HttpContext.Session.Remove("cart");
       
            return RedirectToAction("Index","Product");
        }

    }
}
