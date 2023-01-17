using IDemandApp.Endpoints.Costumers.DTO;

namespace IDemandApp.Endpoints.Costumers;

public class CostumerPost
{
    public static string Template => "/costumers";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(CostumerRequestDTO request, UserRepository userRepository)
    {
        var userClaims = new List<Claim>
        {
            new Claim("Cpf", request.Cpf),
            new Claim("Name", request.Name),
        };

        (IdentityResult identity, string userId) result = await userRepository.CreateUser(request.Email,request.Password, userClaims);
        if (!result.identity.Succeeded)
            return Results.BadRequest(result.identity.Errors);

        return Results.Created($"/costumers/{result.userId}", result.userId);
    }
}
