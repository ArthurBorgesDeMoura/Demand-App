using IDemandApp.Data;
using IDemandApp.Domain.Products;
using IDemandApp.Endpoints.Categories.DTO;
using Microsoft.AspNetCore.Authorization;

namespace IDemandApp.Endpoints.Categories;

public class CategoryPost
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(CategoryRequestDTO request, ApplicationDbContext context)
    {
        var category = new Category(request.Name, "Test", "Test");

        if (!category.IsValid)
        {
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());
        }

        context.Categories.Add(category);
        context.SaveChanges();
        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}
