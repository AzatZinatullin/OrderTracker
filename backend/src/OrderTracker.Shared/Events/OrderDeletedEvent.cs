namespace OrderTracker.Shared.Events;

/// <summary>
/// Событие: Заказ был удален.
/// </summary>
public record OrderDeletedEvent
{
    public Guid OrderId { get; init; }
    public string OrderNumber { get; init; } = string.Empty;
}
