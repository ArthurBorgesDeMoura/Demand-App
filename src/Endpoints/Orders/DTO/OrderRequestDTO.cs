namespace IDemandApp.Endpoints.Orders.DTO;

public record OrderRequestDTO(List<Guid> products, string deliveryAddress );