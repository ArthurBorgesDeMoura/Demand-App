using IDemandApp.Data;
using IDemandApp.Domain.Products;
using Microsoft.AspNetCore.Authorization;

namespace IDemandApp.Endpoints.Categories;

public class CategoryPost
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static IResult Action(CategoryRequestDTO request, ApplicationDbContext context)
    {
        var category = new Category
        {
            Name = request.Name,
            CreatedBy = "Test",
            CreatedAt = DateTime.Now,
            UpdatedBy = "Test",
            UpdatedAt = DateTime.Now,
        };
        context.Categories.Add(category);
        context.SaveChanges();
        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}
