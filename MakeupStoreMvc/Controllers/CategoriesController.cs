using MakeupStoreMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace MakeupStoreMvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly HttpClient _httpClient;

        public CategoriesController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7090/api/");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("Categories/GetCategories");

            if (!response.IsSuccessStatusCode)
            {
                return View(new List<Category>());
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<Category>>(jsonData);

            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            var jsonData = JsonConvert.SerializeObject(category);

            var content = new StringContent(
                jsonData,
                Encoding.UTF8,
                "application/json");

            await _httpClient.PostAsync("Categories/AddCategory", content);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"Categories/GetCategoryById/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<Category>(jsonData);

            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            var jsonData = JsonConvert.SerializeObject(category);

            var content = new StringContent(
                jsonData,
                Encoding.UTF8,
                "application/json");

            await _httpClient.PutAsync($"Categories/UpdateCategory/{category.Id}", content);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _httpClient.DeleteAsync($"Categories/DeleteCategory/{id}");

            return RedirectToAction("Index");
        }
    }
}