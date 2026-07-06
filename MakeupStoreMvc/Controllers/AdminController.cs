using MakeupStoreMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MakeupStoreMvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly UserManager<Users> _userManager;

        public AdminController(UserManager<Users> userManager)
        {
            _userManager = userManager;

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7090/api/");
        }

        public async Task<IActionResult> Index()
        {
            var productResponse = await _httpClient.GetAsync("Products/GetProducts");
            var categoryResponse = await _httpClient.GetAsync("Categories/GetCategories");
            var brandResponse = await _httpClient.GetAsync("Brands/GetBrands");

            var products = new List<ProductListDto>();
            var categories = new List<Category>();
            var brands = new List<Brand>();

            if (productResponse.IsSuccessStatusCode)
            {
                var json = await productResponse.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<List<ProductListDto>>(json);
            }

            if (categoryResponse.IsSuccessStatusCode)
            {
                var json = await categoryResponse.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<List<Category>>(json);
            }

            if (brandResponse.IsSuccessStatusCode)
            {
                var json = await brandResponse.Content.ReadAsStringAsync();
                brands = JsonConvert.DeserializeObject<List<Brand>>(json);
            }

            ViewBag.ProductCount = products.Count;
            ViewBag.CategoryCount = categories.Count;
            ViewBag.BrandCount = brands.Count;
            ViewBag.UserCount = _userManager.Users.Count();

            ViewBag.LowStockProducts = products
                .Where(x => x.Stock <= 30)
                .OrderBy(x => x.Stock)
                .Take(5)
                .ToList();

            ViewBag.LastProducts = products
                .OrderByDescending(x => x.Id)
                .Take(5)
                .ToList();

            return View();
        }
    }
}