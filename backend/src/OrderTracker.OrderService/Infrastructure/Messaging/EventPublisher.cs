using MassTransit;
using OrderTracker.OrderService.Application.Interfaces;
using OrderTracker.Shared.Events;

namespace OrderTracker.OrderService.Infrastructure.Messaging;

/// <summary>
/// Реализация публикатора событий на базе MassTransit.
/// </summary>
public class EventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="EventPublisher"/>.
    /// </summary>
    /// <param name="publishEndpoint">Конечная точка публикации MassTransit.</param>
    public EventPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    /// <summary>
    /// Публикует событие создания заказа в шину данных.
    /// </summary>
    /// <param name="event">Объект события.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task PublishOrderCreatedAsync(OrderCreatedEvent @event, CancellationToken cancellationToken = default)
    {
        await _publishEndpoint.Publish(@event, cancellationToken);
    }

    /// <summary>
    /// Публикует событие изменения статуса заказа в шину данных.
    /// </summary>
    /// <param name="event">Объект события.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task PublishOrderStatusChangedAsync(OrderStatusChangedEvent @event, CancellationToken cancellationToken = default)
    {
        await _publishEndpoint.Publish(@event, cancellationToken);
    }

    /// <summary>
    /// Публикует событие удаления заказа в шину данных.
    /// </summary>
    /// <param name="event">Объект события.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task PublishOrderDeletedAsync(OrderDeletedEvent @event, CancellationToken cancellationToken = default)
    {
        await _publishEndpoint.Publish(@event, cancellationToken);
    }
}
