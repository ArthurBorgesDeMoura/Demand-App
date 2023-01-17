using IDemandApp.Endpoints.Products.DTO;

namespace IDemandApp.Endpoints.Products;

public class ProductPost
{
    public static string Template => "/products";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(ProductRequestDTO request, ApplicationDbContext context, HttpContext http)
    {
        var userId = http.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
        var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == request.categoryId);

        var product = new Product(request.Name, request.Description, category, request.HasStock, userId, request.price);

        if (!product.IsValid)
            return Results.ValidationProblem(product.Notifications.ConvertToProblemDetails());
      
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
        return Results.Created($"/products/{product.Id}", product.Id);
    }
}
