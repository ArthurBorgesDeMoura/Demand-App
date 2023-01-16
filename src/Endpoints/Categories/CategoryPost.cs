using IDemandApp.Data;
using IDemandApp.Domain.Products;
using IDemandApp.Endpoints.Categories.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IDemandApp.Endpoints.Categories;

public class CategoryPost
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]

    public static IResult Action(CategoryRequestDTO request, ApplicationDbContext context, HttpContext http)
    {
        var userId = http.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var category = new Category(request.Name, userId, userId);

        if (!category.IsValid)
        {
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());
        }

        context.Categories.Add(category);
        context.SaveChanges();
        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}
