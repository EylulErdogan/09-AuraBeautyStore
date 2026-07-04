using MakeupStoreApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MakeupStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        [HttpGet("GetBrands")]
        public IActionResult GetBrands()
        {
            var values = Context.GetList<Brand>("SELECT * FROM Brands");
            return Ok(values);
        }

        [HttpGet("GetBrandById/{id}")]
        public IActionResult GetBrandById(int id)
        {
            var value = Context.Get<Brand>(
                "SELECT * FROM Brands WHERE Id = @Id",
                new { Id = id });

            return Ok(value);
        }

        [HttpPost("AddBrand")]
        public IActionResult AddBrand(Brand brand)
        {
            Context.Execute(
                "INSERT INTO Brands(BrandName) VALUES(@BrandName)",
                new { brand.BrandName });

            return Ok("Brand added successfully");
        }

        [HttpPut("UpdateBrand/{id}")]
        public IActionResult UpdateBrand(int id, Brand brand)
        {
            Context.Execute(
                "UPDATE Brands SET BrandName = @BrandName WHERE Id = @Id",
                new
                {
                    Id = id,
                    BrandName = brand.BrandName
                });

            return Ok("Brand updated successfully");
        }

        [HttpDelete("DeleteBrand/{id}")]
        public IActionResult DeleteBrand(int id)
        {
            Context.Execute(
                "DELETE FROM Brands WHERE Id = @Id",
                new { Id = id });

            return Ok("Brand deleted successfully");
        }
    }
}