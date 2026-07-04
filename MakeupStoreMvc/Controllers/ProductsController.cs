using MakeupStoreMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace MakeupStoreMvc.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductsController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7090/api/");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("Products/GetProducts");

            if (!response.IsSuccessStatusCode)
                return View(new List<ProductListDto>());

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ProductListDto>>(jsonData);

            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(product),
                Encoding.UTF8,
                "application/json");

            await _httpClient.PostAsync("Products/AddProduct", content);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            await LoadDropdowns();

            var response = await _httpClient.GetAsync($"Products/GetProductById/{id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var jsonData = await response.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<Product>(jsonData);

            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(product),
                Encoding.UTF8,
                "application/json");

            await _httpClient.PutAsync($"Products/UpdateProduct/{product.Id}", content);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _httpClient.DeleteAsync($"Products/DeleteProduct/{id}");
            return RedirectToAction("Index");
        }

        private async Task LoadDropdowns()
        {
            var categoryResponse = await _httpClient.GetAsync("Categories/GetCategories");
            var brandResponse = await _httpClient.GetAsync("Brands/GetBrands");

            var categoryJson = await categoryResponse.Content.ReadAsStringAsync();
            var brandJson = await brandResponse.Content.ReadAsStringAsync();

            ViewBag.Categories = JsonConvert.DeserializeObject<List<Category>>(categoryJson);
            ViewBag.Brands = JsonConvert.DeserializeObject<List<Brand>>(brandJson);
        }
    }
}