using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using OrderTracker.NotificationService.Hubs;
using OrderTracker.Shared.Events;

namespace OrderTracker.NotificationService.Consumers;

/// <summary>
/// Консьюмер для обработки события создания заказа
/// </summary>
public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly IHubContext<OrderTrackingHub> _hubContext;
    private readonly ILogger<OrderCreatedConsumer> _logger;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="OrderCreatedConsumer"/>.
    /// </summary>
    /// <param name="hubContext">Контекст хаба</param>
    /// <param name="logger">Логгер</param>
    public OrderCreatedConsumer(IHubContext<OrderTrackingHub> hubContext, ILogger<OrderCreatedConsumer> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    /// <summary>
    /// Обрабатывает событие создания заказа.
    /// </summary>
    /// <param name="context">Контекст события.</param>
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        _logger.LogInformation("Received OrderCreatedEvent for Order {OrderId}", context.Message.OrderId);
        
        // Уведомить всех клиентов о создании нового заказа
        await _hubContext.Clients.All.SendAsync("OrderCreated", context.Message);
    }
}
