using OrderTracker.Shared.Enums;

namespace OrderTracker.OrderService.Application.DTOs;

/// <summary>
/// Dto с данными заказа для ответа
/// </summary>
/// <param name="Id">Идентификатор заказа</param>
/// <param name="OrderNumber">Номер заказа</param>
/// <param name="Description">Описание заказа</param>
/// <param name="Status">Статус заказа</param>
/// <param name="CreatedAt">Дата создания</param>
/// <param name="UpdatedAt">Дата обновления</param>
public record OrderResponse(
    Guid Id,
    string OrderNumber,
    string Description,
    OrderStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
