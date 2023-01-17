namespace IDemandApp.Endpoints.Employees;

public class EmployeePost
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(EmployeeRequestDTO request, UserRepository userRepository, HttpContext http)
    {
        var userClaims = new List<Claim>
        {
            new Claim("EmployeeCode", request.EmployeeCode),
            new Claim("Name", request.Name),
            new Claim("CreatedBy", http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value)
        };

        (IdentityResult identity, string userId) result = await userRepository.CreateUser(request.Email, request.Password, userClaims);
        if (!result.identity.Succeeded)
            return Results.BadRequest(result.identity.Errors);

        return Results.Created($"/employees/{result.userId}", result.userId);
    }
}
