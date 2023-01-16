namespace IDemandApp.Data.Repository;

public class UserRepository
{
    private readonly IConfiguration configuration;

    public UserRepository(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task<IEnumerable<EmployeeResponseDTO>> GetAll(int page, int rows)
    {
        var conn = new SqlConnection(configuration["Database:ConnectionString"]);

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
}
