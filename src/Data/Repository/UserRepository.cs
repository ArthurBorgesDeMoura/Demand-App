namespace IDemandApp.Data.Repository;

public class UserRepository
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager;

    public UserRepository(IConfiguration configuration, UserManager<IdentityUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<IEnumerable<EmployeeResponseDTO>> GetAllEmployees(int page, int rows)
    {
        var conn = new SqlConnection(_configuration["Database:ConnectionString"]);

        var query = @"select Email, ClaimValue as Name
                      from AspNetUsers u inner join AspNetUserClaims uc
                      on u.Id = uc.UserId and uc.ClaimType = 'Name'
                      order by Name 
                      offset (@page -1) * @rows rows fetch next @rows rows only";
        return await conn.QueryAsync<EmployeeResponseDTO>(
            query,
            new { page, rows }
            );
    }

    public async Task<(IdentityResult, string)> CreateUser(string email, string password, List<Claim> claims)
    {
        var user = new IdentityUser
        {
            UserName = email,
            Email = email,
        };
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return (result, String.Empty);

        return(await _userManager.AddClaimsAsync(user, claims), user.Id);
    }
}
