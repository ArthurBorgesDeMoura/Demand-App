using System.ComponentModel.DataAnnotations;

namespace IDemandApp.Domain.Products;

public class Category : BaseEntity
{
    [Required]
    public string Name{ get; set; }
    public bool Active { get; set; } = true;
}
