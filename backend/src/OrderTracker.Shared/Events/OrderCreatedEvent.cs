using OrderTracker.Shared.Enums;

namespace OrderTracker.Shared.Events;

/// <summary>
/// Событие: Заказ был успешно создан.
/// </summary>
public record OrderCreatedEvent
{
    public Guid OrderId { get; init; }
    public string OrderNumber { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public OrderStatus Status { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}
