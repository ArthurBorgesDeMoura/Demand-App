using IDemandApp.Endpoints.Orders.DTO;
using IDemandApp.Endpoints.Products.DTO;

namespace IDemandApp.Endpoints.Orders;

public class OrderGetById
{
    public static string Template => "/orders/{id}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize]
    public static async Task<IResult> Action(Guid id, ApplicationDbContext context, HttpContext http)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var isEmployee = http.User.Claims.FirstOrDefault(c => c.Type == "EmployeeCode");
       

        var order = context.Orders.FirstOrDefault(o => o.Id == id);

        if (order == null)
            return Results.NotFound("Order not found");

        if (userId != order.CostumerId && isEmployee == null)
            return Results.Unauthorized();

        var products = order.Products.Select(p => new
        {
            p.Id,
            p.Name,
            p.Description,
            p.Price
        });

        return Results.Ok( new {
            order.Id,
            order.TotalPrice,
            order.DeliveryAddress,
            order.Status,
            Products = products
        });
    }

}
