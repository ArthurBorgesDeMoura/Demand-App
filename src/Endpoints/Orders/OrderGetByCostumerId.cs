using IDemandApp.Endpoints.Orders.DTO;
using IDemandApp.Endpoints.Products.DTO;

namespace IDemandApp.Endpoints.Orders;

public class OrderGetByCostumerId
{
    public static string Template => "/orders/{costumerId}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize]
    public static async Task<IResult> Action( string costumerId, ApplicationDbContext context, HttpContext http,int page = 1, int rows = 10)
    {
        if (rows > 10)
            return Results.Problem(title: "row with max 10", statusCode: 400);

        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var isEmployee = http.User.Claims.FirstOrDefault(c => c.Type == "EmployeeCode");
        if (userId != costumerId && isEmployee == null)
            return Results.Unauthorized();

        var queryBase = context.Orders.Where(o => o.CostumerId == costumerId).Include(o => o.Products).OrderBy(o => o.CreatedAt)
            .Select(o => new {
                o.Id,
                o.TotalPrice,
                o.DeliveryAddress,
                o.Status,
                Products = context.Products.Select(p => new { p.Id, p.Name, p.Description, p.Price }).ToList()                
                });
        var order = queryBase.Skip((page - 1) * rows).Take(rows).ToList();

        return Results.Ok(order);
    }

}
