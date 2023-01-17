namespace IDemandApp.Endpoints.Categories;

public class CategoryGetAll
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;


    public static IResult Action(ApplicationDbContext context)
    {
        var categories = context.Categories.AsNoTracking().ToList();
        var response = categories.Select(c => new CategoryResponseDTO(c.Id, c.Name, c.Active));
        return Results.Ok(response); 
    }
}
