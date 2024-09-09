using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace AspCoreJwtDb.Models;

public class Category
{
    // Unique identifier for the category
    public int Id { get; set; }

    // Name of the category, required and with a maximum length of 100 characters
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; }
}


