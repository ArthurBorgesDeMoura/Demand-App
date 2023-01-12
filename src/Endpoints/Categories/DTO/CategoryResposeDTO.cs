namespace IDemandApp.Endpoints.Categories.DTO;

public class CategoryResposeDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool Active { get; set; } = true;
}
