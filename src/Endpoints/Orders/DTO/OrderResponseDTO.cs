using IDemandApp.Endpoints.Products.DTO;

namespace IDemandApp.Endpoints.Orders.DTO;

public record OrderResponseDTO(List<ProductShowcaseResponseDTO> Products, decimal TotalPrice, string DeliveryAddress, string Status, string costumerName);
