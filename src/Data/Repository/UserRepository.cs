using Dapper;
using IDemandApp.Endpoints.Employees.DTO;
using Microsoft.Data.SqlClient;

namespace IDemandApp.Data.Repository;

public class UserRepository
{
    private readonly IConfiguration configuration;

    public UserRepository(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public IEnumerable<EmployeeResponseDTO> GetAll(int page, int rows)
    {
        var conn = new SqlConnection(configuration["Database:ConnectionString"]);

        var query = @"select Email, ClaimValue as Name
                      from AspNetUsers u inner join AspNetUserClaims uc
                      on u.Id = uc.UserId and uc.ClaimType = 'Name'
                      order by Name 
                      offset (@page -1) * @rows rows fetch next @rows rows only";
        var u = conn.Query<EmployeeResponseDTO>(
            query,
            new { page, rows }
            );
        return u;
    }
}
