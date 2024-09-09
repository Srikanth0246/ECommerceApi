using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AspCoreJwtDb.Models;

namespace AspCoreJwtDb.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // Static list to store products
        private static List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "pen", Price = 25, CategoryId = 1 },
            new Product { Id = 2, Name = "pencil", Price = 5, CategoryId = 1 },
            new Product { Id = 3, Name = "eraser", Price = 4, CategoryId = 1 },
            new Product { Id = 4, Name = "sharpner", Price = 3, CategoryId = 1 },
            new Product { Id = 5, Name = "mouse", Price = 350, CategoryId = 3 },
            new Product { Id = 6, Name = "keyboard", Price = 550, CategoryId = 3 },
            new Product { Id = 7, Name = "milk", Price = 35, CategoryId = 2 },
            new Product { Id = 8, Name = "bread", Price = 55, CategoryId = 2 }
        };

        // GET: api/products
        // Retrieves all products
        [HttpGet]
        public IActionResult Get() => Ok(products);

        [Authorize(Roles = "Admin")]
        // GET: api/products/{id}
        // Retrieves a product by its ID
        [HttpGet("{id}")]
        public IActionResult Get(int id) => Ok(products.FirstOrDefault(p => p.Id == id));

        [Authorize(Roles = "Admin")]
        // POST: api/products
        // Adds a new product
        [HttpPost]
        public IActionResult Post(Product product)
        {
            if (product == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid product data.");
            }

            // Check if a product with the same Id and CategoryId already exists
            var existingProduct = products.FirstOrDefault(p => p.Id == product.Id && p.CategoryId == product.CategoryId);
            if (existingProduct != null)
            {
                return Conflict($"A product with Id '{product.Id}' and CategoryId '{product.CategoryId}' already exists.");
            }

            // Add the product to the list
            products.Add(product);

            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [Authorize(Roles = "Admin")]
        // PUT: api/products/{id}
        // Updates an existing product by its ID
        [HttpPut("{id}")]
        public IActionResult Put(int id, Product product)
        {
            var existingProduct = products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null) return NotFound();

            // Update the product details
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.MfgDate = product.MfgDate;
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        // DELETE: api/products/{id}
        // Deletes a product by its ID
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            // Remove the product from the list
            products.Remove(product);
            return NoContent();
        }
    }
}
