namespace IDemandApp.Endpoints.Products.DTO;

public record ProductResponseDTO(Guid Id, string Name, string CategoryName, string Description, bool HasStock, decimal Price);
public record ProductShowcaseResponseDTO(Guid Id,string Name, string CategoryName, string Description, decimal Price);
