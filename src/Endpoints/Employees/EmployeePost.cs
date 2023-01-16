namespace IDemandApp.Endpoints.Employees;

public class EmployeePost
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(EmployeeRequestDTO request, UserManager<IdentityUser> userManager, HttpContext http)
    {
        var user = new IdentityUser
        {
            UserName = request.Email,
            Email = request.Email,
        };
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        var userClaims = new List<Claim>
        {
            new Claim("EmployeeCode", request.EmployeeCode),
            new Claim("Name", request.Name),
            new Claim("CreatedBy", http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value)
        };

        var claimResult = await userManager.AddClaimsAsync(user, userClaims);
        if (!claimResult.Succeeded)
            return Results.BadRequest(claimResult.Errors);

        return Results.Created($"/employees/{user.Id}", user.Id);
    }
}
