using IDemandApp.Endpoints.Products.DTO;

namespace IDemandApp.Endpoints.Products;

public class ProductGetAll
{
    public static string Template => "/products";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(ApplicationDbContext context, int page = 1, int rows = 10, string orderBy = "name")
    {
        if (rows > 10)
            return Results.Problem(title: "row with max 10", statusCode: 400);

        var queryBase = context.Products.AsNoTracking().Include(p => p.Category).Where(p => p.HasStock && p.Category.Active);

        if (orderBy == "priceASC")
            queryBase = queryBase.OrderBy(p => p.Price);
        else if (orderBy == "priceDESC")
            queryBase = queryBase.OrderByDescending(p => p.Price);
        else if (orderBy == "name")
            queryBase = queryBase.OrderBy(p => p.Name);
        else
            return Results.Problem(title: "Order only by priceASC | priceDESC | name", statusCode: 400);
        var queryFilter = queryBase.Skip((page - 1) * rows).Take(rows);

        var products = context.Products.Include(p => p.Category).OrderBy(p => p.Name).ToList();
        var result = products.Select(p => new ProductResponseDTO(p.Id, p.Name, p.Category.Name, p.Description, p.HasStock, p.Price));
        return Results.Ok(result);
    }

}
