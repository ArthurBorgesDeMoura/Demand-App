using System.ComponentModel.DataAnnotations;

namespace IDemandApp.Domain.Products;

public class Category : BaseEntity
{
    [Required]
    public string Name{ get; set; }
    public bool Request { get; set; } = true;
}
