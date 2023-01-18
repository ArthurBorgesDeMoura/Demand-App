using IDemandApp.Domain.Orders;
using IDemandApp.Endpoints.Orders.DTO;

namespace IDemandApp.Endpoints.Orders;

public class OrderPost
{
    public static string Template => "/orders";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "CostumerPolicy")]
    public static async Task<IResult> Action(OrderRequestDTO request, ApplicationDbContext context, HttpContext http)
    {
        var products = context.Products.Where(p => request.products.Contains(p.Id)).ToList();

        var order = new Order(http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value,
            http.User.Claims.First(c => c.Type == "Name").Value,
            products, request.deliveryAddress);
        if (!order.IsValid)
            return Results.ValidationProblem(order.Notifications.ConvertToProblemDetails());

        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        return Results.Created($"/orders/{order.Id}", order.Id);
    }
}
