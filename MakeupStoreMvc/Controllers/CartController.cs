using MakeupStoreMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace MakeupStoreMvc.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient _httpClient;

        public CartController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7090/api/");
        }

        // View Cart Page
        public IActionResult Index()
        {
            var cart = GetCartFromSession();
            return View(cart);
        }

        // Add Item to Cart
        [HttpPost]
        public async Task<IActionResult> Add(int productId, string color, string size, int quantity)
        {
            // 1. Fetch products from API to get accurate details (price, name, image)
            var response = await _httpClient.GetAsync("Products/GetProducts");
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<ProductListDto>>(jsonData);
            var product = products?.FirstOrDefault(x => x.Id == productId);

            if (product == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // 2. Retrieve existing cart
            var cart = GetCartFromSession();

            // 3. Find if item already exists in cart with same configurations
            var existingItem = cart.FirstOrDefault(x => 
                x.ProductId == productId && 
                x.Color.Equals(color, StringComparison.OrdinalIgnoreCase) && 
                x.Size.Equals(size, StringComparison.OrdinalIgnoreCase)
            );

            if (existingItem != null)
            {
                // Increment quantity, make sure it doesn't exceed stock limit
                int newQty = existingItem.Quantity + quantity;
                existingItem.Quantity = newQty > product.Stock ? product.Stock : newQty;
            }
            else
            {
                // Add new item
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Quantity = quantity > product.Stock ? product.Stock : quantity,
                    Color = color,
                    Size = size
                });
            }

            // 4. Save updated cart back to session
            SaveCartToSession(cart);

            return RedirectToAction("Index");
        }

        // Remove Item from Cart
        public IActionResult Remove(int productId, string color, string size)
        {
            var cart = GetCartFromSession();

            var itemToRemove = cart.FirstOrDefault(x => 
                x.ProductId == productId && 
                x.Color.Equals(color, StringComparison.OrdinalIgnoreCase) && 
                x.Size.Equals(size, StringComparison.OrdinalIgnoreCase)
            );

            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                SaveCartToSession(cart);
            }

            return RedirectToAction("Index");
        }

        // Checkout Action: Places the order and clears the session
        [HttpPost]
        public IActionResult Checkout()
        {
            var cart = GetCartFromSession();
            if (!cart.Any())
            {
                return RedirectToAction("Index");
            }

            // Calculate details
            var total = cart.Sum(x => x.Price * x.Quantity);
            var orderNumber = "AURA-" + new Random().Next(100000, 999999).ToString();

            // Store summary in TempData as JSON
            var summary = new OrderSuccessViewModel
            {
                OrderNumber = orderNumber,
                TotalAmount = total,
                Items = cart
            };

            TempData["OrderSummary"] = JsonConvert.SerializeObject(summary);

            // Empty the cart
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Success");
        }

        // Success Action: Displays the OrderConfirmation page
        public IActionResult Success()
        {
            var summaryJson = TempData["OrderSummary"] as string;
            if (string.IsNullOrEmpty(summaryJson))
            {
                return RedirectToAction("Index", "Home");
            }

            var summary = JsonConvert.DeserializeObject<OrderSuccessViewModel>(summaryJson);
            return View(summary);
        }

        // Helper to load Cart from Session
        private List<CartItem> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItem>();
            }
            return JsonConvert.DeserializeObject<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }

        // Helper to save Cart to Session
        private void SaveCartToSession(List<CartItem> cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("Cart", cartJson);
        }
    }
}
