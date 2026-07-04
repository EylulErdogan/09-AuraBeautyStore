using MakeupStoreMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace MakeupStoreMvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BrandsController : Controller
    {
        private readonly HttpClient _httpClient;

        public BrandsController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7090/api/");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("Brands/GetBrands");

            if (!response.IsSuccessStatusCode)
                return View(new List<Brand>());

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<Brand>>(jsonData);

            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Brand brand)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(brand),
                Encoding.UTF8,
                "application/json");

            await _httpClient.PostAsync("Brands/AddBrand", content);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"Brands/GetBrandById/{id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var jsonData = await response.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<Brand>(jsonData);

            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Brand brand)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(brand),
                Encoding.UTF8,
                "application/json");

            await _httpClient.PutAsync($"Brands/UpdateBrand/{brand.Id}", content);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _httpClient.DeleteAsync($"Brands/DeleteBrand/{id}");
            return RedirectToAction("Index");
        }
    }
}