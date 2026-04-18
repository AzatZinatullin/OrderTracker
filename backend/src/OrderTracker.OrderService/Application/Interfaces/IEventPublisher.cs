using OrderTracker.Shared.Events;

namespace OrderTracker.OrderService.Application.Interfaces;

/// <summary>
/// Контракт для публикации событий в шину данных.  
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Публикует событие создания заказа в шину данных.
    /// </summary>
    /// <param name="@event">Объект события.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task PublishOrderCreatedAsync(OrderCreatedEvent @event, CancellationToken cancellationToken = default);

    /// <summary>
    /// Публикует событие изменения статуса заказа в шину данных.
    /// </summary>
    /// <param name="@event">Объект события.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task PublishOrderStatusChangedAsync(OrderStatusChangedEvent @event, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Публикует событие удаления заказа в шину данных.
    /// </summary>
    /// <param name="@event">Объект события.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task PublishOrderDeletedAsync(OrderDeletedEvent @event, CancellationToken cancellationToken = default);
}
