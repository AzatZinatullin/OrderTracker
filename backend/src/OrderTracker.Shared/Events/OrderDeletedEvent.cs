namespace OrderTracker.Shared.Events;

/// <summary>
/// Событие: Заказ был удален.
/// </summary>
public record OrderDeletedEvent
{
    /// <summary>
    /// Идентификатор заказа.
    /// </summary>
    public Guid OrderId { get; init; }

    /// <summary>
    /// Номер заказа.
    /// </summary>
    public string OrderNumber { get; init; } = string.Empty;
}
