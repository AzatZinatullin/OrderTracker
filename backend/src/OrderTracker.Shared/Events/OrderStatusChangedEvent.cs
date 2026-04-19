using OrderTracker.Shared.Enums;

namespace OrderTracker.Shared.Events;

/// <summary>
/// Событие: Статус заказа был изменён.
/// </summary>
public record OrderStatusChangedEvent
{
    /// <summary>
    /// Идентификатор заказа.
    /// </summary>
    public Guid OrderId { get; init; }

    /// <summary>
    /// Номер заказа.
    /// </summary>
    public string OrderNumber { get; init; } = string.Empty;

    /// <summary>
    /// Предыдущий статус заказа.
    /// </summary>
    public OrderStatus PreviousStatus { get; init; }

    /// <summary>
    /// Новый статус заказа.
    /// </summary>
    public OrderStatus NewStatus { get; init; }

    /// <summary>
    /// Дата и время изменения статуса заказа.
    /// </summary>
    public DateTimeOffset UpdatedAt { get; init; }
}
