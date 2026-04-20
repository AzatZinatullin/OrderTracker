using Microsoft.AspNetCore.SignalR;

namespace OrderTracker.NotificationService.Hubs;

/// <summary>
/// Хаб для отслеживания заказов.
/// </summary>
public class OrderTrackingHub : Hub
{
    private readonly ILogger<OrderTrackingHub> _logger;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="OrderTrackingHub"/>.
    /// </summary>
    /// <param name="logger">Логгер.</param>
    public OrderTrackingHub(ILogger<OrderTrackingHub> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Вызывается при подключении нового клиента.
    /// </summary>
    /// <returns>Задача, представляющая асинхронное подключение.</returns>
    public override Task OnConnectedAsync()
    {
        _logger.LogInformation("Подключен клиент: {ConnectionId}", Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    /// <summary>
    /// Вызывается при отключении клиента.
    /// </summary>
    /// <param name="exception">Исключение, вызвавшее отключение (если есть).</param>
    /// <returns>Задача, представляющая асинхронное отключение.</returns>
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Отключен клиент: {ConnectionId}", Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Подписывает клиента на обновления по конкретному заказу через механизм групп.
    /// </summary>
    /// <param name="orderId">Идентификатор заказа.</param>
    public async Task JoinOrderGroup(string orderId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"order-{orderId}");
        _logger.LogInformation("Клиент {ConnectionId} присоединился к группе order-{OrderId}", Context.ConnectionId, orderId);
    }

    /// <summary>
    /// Отписывает клиента от обновлений по конкретному заказу.
    /// </summary>
    /// <param name="orderId">Идентификатор заказа.</param>
    public async Task LeaveOrderGroup(string orderId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"order-{orderId}");
        _logger.LogInformation("Клиент {ConnectionId} покинул группу order-{OrderId}", Context.ConnectionId, orderId);
    }
}
