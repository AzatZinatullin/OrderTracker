using OrderTracker.Shared.Enums;

namespace OrderTracker.Shared.Events;

/// <summary>
/// Событие: Заказ был успешно создан.
/// </summary>
public record OrderCreatedEvent
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
    /// Описание заказа.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Статус заказа.
    /// </summary>
    public OrderStatus Status { get; init; }

    /// <summary>
    /// Дата и время создания заказа.
    /// </summary>
    public DateTimeOffset CreatedAt { get; init; }
}
