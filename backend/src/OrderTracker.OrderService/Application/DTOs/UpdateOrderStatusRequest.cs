using OrderTracker.Shared.Enums;

namespace OrderTracker.OrderService.Application.DTOs;

/// <summary>
/// Dto на обновление статуса заказа
/// </summary>
/// <param name="NewStatus">Новый статус заказа</param>
public record UpdateOrderStatusRequest(OrderStatus NewStatus);
