using MakeupStoreApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MakeupStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        [HttpGet("GetCategories")]
        public IActionResult GetCategories()
        {
            var values = Context.GetList<Category>("SELECT * FROM Categories");
            return Ok(values);
        }

        [HttpGet("GetCategoryById/{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var value = Context.Get<Category>(
                "SELECT * FROM Categories WHERE Id = @Id",
                new { Id = id });

            return Ok(value);
        }

        [HttpPost("AddCategory")]
        public IActionResult AddCategory(Category category)
        {
            Context.Execute(
                "INSERT INTO Categories(CategoryName) VALUES(@CategoryName)",
                new { category.CategoryName });

            return Ok("Category added successfully");
        }

        [HttpPut("UpdateCategory/{id}")]
        public IActionResult UpdateCategory(int id, Category category)
        {
            Context.Execute(
                "UPDATE Categories SET CategoryName = @CategoryName WHERE Id = @Id",
                new
                {
                    Id = id,
                    CategoryName = category.CategoryName
                });

            return Ok("Category updated successfully");
        }

        [HttpDelete("DeleteCategory/{id}")]
        public IActionResult DeleteCategory(int id)
        {
            Context.Execute(
                "DELETE FROM Categories WHERE Id = @Id",
                new { Id = id });

            return Ok("Category deleted successfully");
        }
    }
}