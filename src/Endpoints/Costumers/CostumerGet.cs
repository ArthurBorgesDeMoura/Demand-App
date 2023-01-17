using IDemandApp.Endpoints.Costumers.DTO;

namespace IDemandApp.Endpoints.Costumers;

public class CostumerGet
{
    public static string Template => "/costumers";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "CostumerPolicy")]
    public static async Task<IResult> Action(HttpContext http)
    {
        var user = http.User;

        var result = new
        {
            Id = user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value,
            Name = user.Claims.First(c => c.Type == "Name").Value,
            Email = user.Claims.First(c => c.Type == ClaimTypes.Email).Value,
            Cpf = user.Claims.First(c => c.Type == "Cpf").Value
        };
        return Results.Ok(result);
    }
}
