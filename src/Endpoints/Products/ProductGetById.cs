using IDemandApp.Endpoints.Products.DTO;

namespace IDemandApp.Endpoints.Products;

public class ProductGetById
{
    public static string Template => "/products/{id}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(Guid id, ApplicationDbContext context, HttpContext http)
    {
        var products = context.Products.AsNoTracking().Where(p => p.Id == id).Include(p => p.Category).OrderBy(p => p.Name).ToList();
        var result = products.Select(p => new ProductResponseDTO(p.Id, p.Name, p.Category.Name, p.Description, p.HasStock, p.Price));
        return Results.Ok(result);
    }

}
