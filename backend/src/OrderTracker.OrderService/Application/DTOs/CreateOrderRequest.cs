namespace OrderTracker.OrderService.Application.DTOs;

/// <summary>
/// Dto на создание заказа
/// </summary>
/// <param name="Description">Описание заказа</param>
public record CreateOrderRequest(string Description);
