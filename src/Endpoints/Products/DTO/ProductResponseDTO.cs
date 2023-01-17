namespace IDemandApp.Endpoints.Products.DTO;

public record ProductResponseDTO(Guid id, string Name, string CategoryName, string Description, bool HasStock, decimal price);
public record ProductShowcaseResponseDTO(string Name, string CategoryName, string Description, decimal price);
