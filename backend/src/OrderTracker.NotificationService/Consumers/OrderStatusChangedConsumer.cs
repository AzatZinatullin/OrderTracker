using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using OrderTracker.NotificationService.Hubs;
using OrderTracker.Shared.Events;

namespace OrderTracker.NotificationService.Consumers;

/// <summary>
/// Консьюмер для обработки события изменения статуса заказа
/// </summary>
public class OrderStatusChangedConsumer : IConsumer<OrderStatusChangedEvent>
{
    private readonly IHubContext<OrderTrackingHub> _hubContext;
    private readonly ILogger<OrderStatusChangedConsumer> _logger;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="OrderStatusChangedConsumer"/>.
    /// </summary>
    /// <param name="hubContext">Контекст хаба SignalR для отправки уведомлений.</param>
    /// <param name="logger">Логгер для отслеживания процесса обработки событий.</param>
    public OrderStatusChangedConsumer(IHubContext<OrderTrackingHub> hubContext, ILogger<OrderStatusChangedConsumer> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    /// <summary>
    /// Обрабатывает полученное событие изменения статуса заказа.
    /// </summary>
    /// <param name="context">Контекст сообщения из очереди.</param>
    /// <returns>Задача, представляющая асинхронную операцию обработки.</returns>
    public async Task Consume(ConsumeContext<OrderStatusChangedEvent> context)
    {
        _logger.LogInformation("Received OrderStatusChangedEvent: {OrderId} changed to {NewStatus}", context.Message.OrderId, context.Message.NewStatus);
            
        // Уведомляем клиентов, просматривающих конкретный заказ
        await _hubContext.Clients.Group($"order-{context.Message.OrderId}").SendAsync("OrderStatusUpdated", context.Message);

        // Уведомляем всех клиентов для обновлений в общем списке
        await _hubContext.Clients.All.SendAsync("OrderStatusUpdatedAll", context.Message);
    }
}
