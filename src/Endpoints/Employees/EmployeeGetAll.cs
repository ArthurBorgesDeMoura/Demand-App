namespace IDemandApp.Endpoints.Employees;

[Authorize(Policy = "EmployeePolicy")]

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action(int page, int rows, UserRepository userRepository)
    {        
        return Results.Ok(await userRepository.GetAllEmployees(page, rows));
    }
}
