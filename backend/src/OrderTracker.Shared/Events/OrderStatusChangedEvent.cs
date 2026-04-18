using OrderTracker.Shared.Enums;

namespace OrderTracker.Shared.Events;

/// <summary>
/// Событие: Статус заказа был изменён.
/// </summary>
public record OrderStatusChangedEvent
{
    public Guid OrderId { get; init; }
    public string OrderNumber { get; init; } = string.Empty;
    public OrderStatus PreviousStatus { get; init; }
    public OrderStatus NewStatus { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}
