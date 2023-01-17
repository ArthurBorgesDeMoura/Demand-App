using System.ComponentModel.DataAnnotations.Schema;

namespace IDemandApp.Domain.Products;

public class Product : BaseEntity
{
    [Required]
    public string Name { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }
    [MaxLength(255)]
    public string Description { get; private set; }
    public bool HasStock { get; private set; }
    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; private set; }

    private Product() { }

    public Product(string name, string description, Category category, bool hasStock, string createdBy, decimal price)
    {
        Name = name;
        Description = description;
        Category = category;
        HasStock = hasStock;

        CreatedAt = DateTime.Now;
        CreatedBy = createdBy;
        UpdatedAt = DateTime.Now;
        UpdatedBy = createdBy;
        Price = price;
        Validate();

    }

    private void Validate()
    {
        var contract = new Contract<Product>()
            .IsNotNullOrEmpty(Name, "Name")
            .IsGreaterOrEqualsThan(Name, 3, "Name")
            .IsNotNull(Category, "Category", "Category not found")
            .IsGreaterThan(Price, 0, "Price")
            .IsNotNullOrEmpty(CreatedBy, "CreatedBy")
            .IsNotNullOrEmpty(UpdatedBy, "UpdatedBy");
        AddNotifications(contract);
    }
}
