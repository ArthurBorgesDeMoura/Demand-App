using IDemandApp.Endpoints.Products.DTO;

namespace IDemandApp.Endpoints.Products;

public class ProductGetReport
{
    public static string Template => "/products/report";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(ProductRepository productRepository, int page = 1, int rows = 5)
    {
        if(rows > 10)
            Results.Problem(title: "row with max 10", statusCode: 400);
        try
        {
            var result = await productRepository.GetReport(page, rows);
            return Results.Ok(result);
        }
        catch (Exception ex)
        {

            throw;
        }
        
    }

}
