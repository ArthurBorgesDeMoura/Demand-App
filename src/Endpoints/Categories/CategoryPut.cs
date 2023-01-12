using IDemandApp.Data;
using IDemandApp.Endpoints.Categories.DTO;

namespace IDemandApp.Endpoints.Categories;

public class CategoryPut
{
    public static string Template => "/categories/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(Guid id, CategoryRequestDTO request, ApplicationDbContext context)
    {
        var category = context.Categories.Where(c => c.Id == id).FirstOrDefault();

        if (category == null)
        {
            return Results.NotFound();
        }

        category.EditInfo(request.Name, request.Active);
        if (!category.IsValid)
        {
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());

        }

        context.SaveChanges();
        return Results.Ok();
    }
}
