namespace IDemandApp.Domain.Products;

public class Category : BaseEntity
{
    [Required]
    public string Name { get; private set; }
    public bool Active { get; private set; } = true;


    public Category(string name, string createdBy, string updatedBy)
    {
        Name = name;
        Active = true;
        CreatedBy = createdBy;
        UpdatedBy = updatedBy;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        Validate();
    }

    public void EditInfo(string name, bool active, string updatedBy)
    {
        Name = name;
        Active = active;
        UpdatedBy = updatedBy;
        UpdatedAt = DateTime.Now;
        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Category>()
            .IsNotNullOrEmpty(Name, "Name")
            .IsGreaterOrEqualsThan(Name, 3, "Name")
            .IsNotNullOrEmpty(CreatedBy, "CreatedBy")
            .IsNotNullOrEmpty(UpdatedBy, "UpdatedBy");
        AddNotifications(contract);
    }
}
