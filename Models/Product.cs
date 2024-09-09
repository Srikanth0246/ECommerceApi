using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AspCoreJwtDb.Models;

public class Product
{
    // Unique identifier for the product
    public int Id { get; set; }
    
    // Name of the product, required and with length constraints
    [Required(ErrorMessage = "Name should be provided")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 30 characters.")]
    public string Name { get; set; }

    // Price of the product, required and within a specified range
    [Required]
    [Range(1, 1000000)]
    public int Price { get; set; }

    // Manufacturing date of the product, required and validated to ensure it's not a future date
    [Required(ErrorMessage = "Manufacturing Date is required.")]
    [MfgDateValidation(ErrorMessage = "Manufacturing Date cannot be a future date.")]
    public DateTime MfgDate { get; set; }

    // Category ID of the product, required
    [Required(ErrorMessage = "Category is required.")]
    public int CategoryId { get; set; }
}

// Custom validation attribute to ensure the manufacturing date is not a future date
public class MfgDateValidationAttribute : ValidationAttribute
{
    // Override the IsValid method to provide custom validation logic
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // Check if the value is a DateTime object and if it's a future date
        if (value is DateTime mfgDate && mfgDate > DateTime.Now)
        {
            return new ValidationResult(ErrorMessage ?? "Manufacturing Date cannot be a future date.");
        }
        // If the date is valid, return success
        return ValidationResult.Success;
    }
}
