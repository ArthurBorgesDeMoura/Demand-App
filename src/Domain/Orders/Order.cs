using System.ComponentModel.DataAnnotations.Schema;

namespace IDemandApp.Domain.Orders;

public class Order : BaseEntity
{
    [Required]
    public string CostumerId { get; private set; }
    public List<Product> Products { get; private set; }
    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; private set; } = 0;
    public string DeliveryAddress { get; private set; }
    public string Status {  get; private set; }

    private Order() { }

    public Order(string costumerId, string costumerName, List<Product> products,  string deliveryAddress)
    {
        CostumerId = costumerId;
        Products = products;
        DeliveryAddress = deliveryAddress;
        CreatedAt = DateTime.Now;
        CreatedBy = costumerName;
        UpdatedAt = DateTime.Now;
        UpdatedBy = costumerName;
        Status = "Waiting for authorization to dispatch";

        foreach (Product product in products)
            TotalPrice += product.Price;

        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Order>()
            .IsNotNullOrEmpty(CostumerId, "CostumerId")
            .IsTrue(Products != null && Products.Any(), "Products")
            .IsGreaterThan(TotalPrice, 0, "Total price")
            .IsNotNullOrEmpty(DeliveryAddress, "Delivary address");
        AddNotifications(contract);
    }
}
