using MakeupStoreApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MakeupStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        [HttpGet("GetCustomers")]
        public IActionResult GetCustomers()
        {
            var values = Context.GetList<Customer>("SELECT * FROM Customers");
            return Ok(values);
        }

        [HttpGet("GetCustomerById/{id}")]
        public IActionResult GetCustomerById(int id)
        {
            var value = Context.Get<Customer>(
                "SELECT * FROM Customers WHERE Id = @Id",
                new { Id = id });

            return Ok(value);
        }

        [HttpPost("AddCustomer")]
        public IActionResult AddCustomer(Customer customer)
        {
            Context.Execute(
                "INSERT INTO Customers(FullName, Email, Phone) VALUES(@FullName, @Email, @Phone)",
                new
                {
                    customer.FullName,
                    customer.Email,
                    customer.Phone
                });

            return Ok("Customer added successfully");
        }

        [HttpPut("UpdateCustomer/{id}")]
        public IActionResult UpdateCustomer(int id, Customer customer)
        {
            Context.Execute(
                "UPDATE Customers SET FullName = @FullName, Email = @Email, Phone = @Phone WHERE Id = @Id",
                new
                {
                    Id = id,
                    customer.FullName,
                    customer.Email,
                    customer.Phone
                });

            return Ok("Customer updated successfully");
        }

        [HttpDelete("DeleteCustomer/{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            Context.Execute(
                "DELETE FROM Customers WHERE Id = @Id",
                new { Id = id });

            return Ok("Customer deleted successfully");
        }
    }
}