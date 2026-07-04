using MakeupStoreApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MakeupStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet("GetProducts")]
        public IActionResult GetProducts()
        {
            string query = @"
                SELECT 
                    p.Id,
                    p.ProductName,
                    p.Price,
                    p.Stock,
                    p.ImageUrl,
                    c.CategoryName,
                    b.BrandName
                FROM Products p
                INNER JOIN Categories c ON p.CategoryId = c.Id
                INNER JOIN Brands b ON p.BrandId = b.Id";

            var values = Context.GetList<ProductListDto>(query);
            return Ok(values);
        }

        [HttpGet("GetProductById/{id}")]
        public IActionResult GetProductById(int id)
        {
            string query = "SELECT * FROM Products WHERE Id = @Id";

            var value = Context.Get<Product>(
                query,
                new { Id = id });

            return Ok(value);
        }

        [HttpPost("AddProduct")]
        public IActionResult AddProduct(Product product)
        {
            string query = @"
                INSERT INTO Products
                (ProductName, Price, Stock, ImageUrl, CategoryId, BrandId)
                VALUES
                (@ProductName, @Price, @Stock, @ImageUrl, @CategoryId, @BrandId)";

            Context.Execute(query, new
            {
                product.ProductName,
                product.Price,
                product.Stock,
                product.ImageUrl,
                product.CategoryId,
                product.BrandId
            });

            return Ok("Product added successfully");
        }

        [HttpPut("UpdateProduct/{id}")]
        public IActionResult UpdateProduct(int id, Product product)
        {
            string query = @"
                UPDATE Products SET
                    ProductName = @ProductName,
                    Price = @Price,
                    Stock = @Stock,
                    ImageUrl = @ImageUrl,
                    CategoryId = @CategoryId,
                    BrandId = @BrandId
                WHERE Id = @Id";

            Context.Execute(query, new
            {
                Id = id,
                product.ProductName,
                product.Price,
                product.Stock,
                product.ImageUrl,
                product.CategoryId,
                product.BrandId
            });

            return Ok("Product updated successfully");
        }

        [HttpDelete("DeleteProduct/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            string query = "DELETE FROM Products WHERE Id = @Id";

            Context.Execute(query, new { Id = id });

            return Ok("Product deleted successfully");
        }
    }
}