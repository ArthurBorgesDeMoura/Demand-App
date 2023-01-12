using System.ComponentModel.DataAnnotations;

namespace IDemandApp.Domain.Products;

public class Product : BaseEntity
{
    [Required]
    public string Name { get; set; }
    [MaxLength(255)]
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public bool HasStock { get; set; }
}
