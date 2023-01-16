using IDemandApp.Data;
using IDemandApp.Domain.Products;
using IDemandApp.Endpoints.Employees.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IDemandApp.Endpoints.Employees;

public class EmployeePost
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static IResult Action(EmployeeRequestDTO request, UserManager<IdentityUser> userManager, HttpContext http)
    {
        var user = new IdentityUser
        {
            UserName = request.Email,
            Email = request.Email,
        };
        var result = userManager.CreateAsync(user, request.Password).Result;
        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        var userClaims = new List<Claim>
        {
            new Claim("EmployeeCode", request.EmployeeCode),
            new Claim("Name", request.Name),
            new Claim("CreatedBy", http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value)
        };

        var claimResult = userManager.AddClaimsAsync(user, userClaims).Result;
        if (!claimResult.Succeeded)
            return Results.BadRequest(claimResult.Errors);

        //if (!category.IsValid)
        //{
        //    return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());
        //}

        return Results.Created($"/employees/{user.Id}", user.Id);
    }
}
