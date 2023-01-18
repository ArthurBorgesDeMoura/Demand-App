using IDemandApp.Endpoints.Products.DTO;

namespace IDemandApp.Endpoints.Products;

public class ProductGetShowcase
{
    public static string Template => "/products/showcase";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(ApplicationDbContext context, int page = 1, int rows = 5, string orderBy ="name")
    {
        if (rows > 5)
            return Results.Problem(title: "row with max 5", statusCode: 400);

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

        var products = queryFilter.ToList();
        var result = products.Select(p => new ProductShowcaseResponseDTO(p.Id, p.Name, p.Category.Name, p.Description, p.Price));
        return Results.Ok(result);
    }

}
