using IDemandApp.Endpoints.Orders.DTO;

namespace IDemandApp.Data.Repository;

public class ProductRepository
{
    private readonly IConfiguration _configuration;

    public ProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<ProductReportResponseDTO>> GetReport(int page, int rows)
    {
        var conn = new SqlConnection(_configuration["Database:ConnectionString"]);

        var query = @"select p.id, p.Name, count(*) Quantity 
                      from Products p inner join OrderProducts op 
                      on op.ProductsId = p.id 
                      group by  p.id, p.Name 
                      order by Quantity desc
                      offset (@page -1) * @rows rows fetch next @rows rows only";
        var result = await conn.QueryAsync<ProductReportResponseDTO>(
       query,
       new { page, rows }
       );

        return result;
    }
}
