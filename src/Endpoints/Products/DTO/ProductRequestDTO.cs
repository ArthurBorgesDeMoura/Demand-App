namespace IDemandApp.Endpoints.Products.DTO;

public record ProductRequestDTO(string Name, string Description, Guid categoryId, bool HasStock, decimal price);
