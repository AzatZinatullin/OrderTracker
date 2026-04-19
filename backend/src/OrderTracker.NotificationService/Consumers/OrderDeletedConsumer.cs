using MassTransit;
using Microsoft.AspNetCore.SignalR;
using OrderTracker.NotificationService.Hubs;
using OrderTracker.Shared.Events;

namespace OrderTracker.NotificationService.Consumers;

/// <summary>
/// Консьюмер для обработки события удаления заказа
/// </summary>
public class OrderDeletedConsumer : IConsumer<OrderDeletedEvent>
{
    private readonly IHubContext<OrderTrackingHub> _hubContext;
    private readonly ILogger<OrderDeletedConsumer> _logger;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="OrderDeletedConsumer"/>.
    /// </summary>
    /// <param name="hubContext">Контекст хаба.</param>
    /// <param name="logger">Логгер.</param>
    public OrderDeletedConsumer(IHubContext<OrderTrackingHub> hubContext, ILogger<OrderDeletedConsumer> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    /// <summary>
    /// Обрабатывает событие удаления заказа.
    /// </summary>
    /// <param name="context">Контекст события.</param>
    public async Task Consume(ConsumeContext<OrderDeletedEvent> context)
    {
        _logger.LogInformation("Consuming OrderDeletedEvent for Order {OrderId}", context.Message.OrderId);

        // Уведомить всех клиентов об удалении
        await _hubContext.Clients.All.SendAsync("OrderDeleted", context.Message);
        
        // Уведомить конкретную группу (если есть слушатели)
        await _hubContext.Clients.Group($"order-{context.Message.OrderId}")
            .SendAsync("OrderDeleted", context.Message);
    }
}
