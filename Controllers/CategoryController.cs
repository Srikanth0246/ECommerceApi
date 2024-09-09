using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AspCoreJwtDb.Models;

namespace AspCoreJwtDb.Controllers{
[Authorize]

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    // Static list to store categories
    private static List<Category> categories = new List<Category>
    {
        new Category { Id = 1, Name = "Stationary" },
        new Category { Id = 2, Name = "Grocery" },
        new Category { Id = 3, Name = "Electronics" }
    };

    // GET: api/categories
    // Retrieves all categories
    [HttpGet]
    public IActionResult Get() => Ok(categories);

    // GET: api/categories/{id}
    // Retrieves a category by its ID
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        return Ok(categories.FirstOrDefault(c => c.Id == id));
    }

    // POST: api/categories
    // Adds a new category
    [HttpPost]
    public IActionResult Post(Category category)
    {
        // Check if a category with the same ID already exists
        if (categories.Any(c => c.Id == category.Id))
        {
            return Conflict($"A category with the ID '{category.Id}' already exists.");
        }

        // Check if a category with the same name already exists
        if (categories.Any(c => c.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)))
        {
            return Conflict($"A category with the name '{category.Name}' already exists.");
        }

        // Add the new category to the list
        categories.Add(category);
        return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
    }

    // PUT: api/categories/{id}
    // Updates an existing category by its ID
    [HttpPut("{id}")]
    public IActionResult Put(int id, Category category)
    {
        var existingCategory = categories.FirstOrDefault(c => c.Id == id);
        if (existingCategory == null) return NotFound();

        // Update the name of the existing category
        existingCategory.Name = category.Name;
        return NoContent();
    }

    // DELETE: api/categories/{id}
    // Deletes a category by its ID
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var category = categories.FirstOrDefault(c => c.Id == id);
        if (category == null) return NotFound();

        // Remove the category from the list
        categories.Remove(category);
        return NoContent();
    }
}
}