using OrderTracker.OrderService.Application.DTOs;
using OrderTracker.OrderService.Domain.Entities;

namespace OrderTracker.OrderService.Application.Mapping;

/// <summary>
/// Расширения для маппинга заказов
/// </summary>
public static class OrderMappingExtensions
{
    /// <summary>
    /// Маппинг заказа в ответ
    /// </summary>
    /// <param name="order">Заказ</param>
    /// <returns>OrderResponse dto с информацией о заказе</returns>
    public static OrderResponse ToResponse(this Order order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));

        return new OrderResponse(
            order.Id,
            order.OrderNumber,
            order.Description,
            order.Status,
            order.CreatedAt,
            order.UpdatedAt
        );
    }
}
