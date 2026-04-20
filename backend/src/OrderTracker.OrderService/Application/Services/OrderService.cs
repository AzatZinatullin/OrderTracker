using OrderTracker.OrderService.Application.DTOs;
using OrderTracker.OrderService.Application.Interfaces;
using OrderTracker.OrderService.Application.Mapping;
using OrderTracker.OrderService.Domain.Entities;
using OrderTracker.Shared.Events;
using OrderTracker.Shared.Exceptions;

namespace OrderTracker.OrderService.Application.Services;

/// <summary>
/// Сервис заказов
/// </summary>
public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<OrderService> _logger;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="OrderService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий заказов</param>
    /// <param name="eventPublisher">Издатель событий</param>
    /// <param name="logger">Логгер</param>
    public OrderService(
        IOrderRepository repository,
        IEventPublisher eventPublisher,
        ILogger<OrderService> logger)
    {
        _repository = repository;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    /// <summary>
    /// Создание нового заказа
    /// </summary>
    /// <param name="request">Запрос на создание заказа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Информация о созданном заказ</returns>
    public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var order = new Order(request.Description);

        await _repository.AddAsync(order, cancellationToken);
        var createdEvent = new OrderCreatedEvent
        {
            OrderId = order.Id,
            OrderNumber = order.OrderNumber,
            Description = order.Description,
            Status = order.Status,
            CreatedAt = order.CreatedAt
        };

        await _eventPublisher.PublishOrderCreatedAsync(createdEvent, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Заказ {OrderId} создан", order.Id);

        return order.ToResponse();
    }

    /// <summary>
    /// Получение всех заказов
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список заказов</returns>
    public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _repository.GetAllAsync(cancellationToken);
        return orders.Select(o => o.ToResponse());
    }

    /// <summary>
    /// Получение заказа по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Информацией о заказе</returns>
    public async Task<OrderResponse?> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _repository.GetByIdAsync(id, cancellationToken);
        return order?.ToResponse();
    }

    /// <summary>
    /// Обновление статуса заказа
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="request">Запрос на обновление статуса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task UpdateOrderStatusAsync(Guid id, UpdateOrderStatusRequest request, CancellationToken cancellationToken = default)
    {
        var order = await _repository.GetByIdAsync(id, cancellationToken);
        if (order == null)
        {
            throw new NotFoundException($"Заказ с id {id} не найден.");
        }

        var previousStatus = order.Status;
        order.UpdateStatus(request.NewStatus);

        if (previousStatus != order.Status)
        {
            await _repository.UpdateAsync(order, cancellationToken);

            var statusChangedEvent = new OrderStatusChangedEvent
            {
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                PreviousStatus = previousStatus,
                NewStatus = order.Status,
                UpdatedAt = order.UpdatedAt
            };

            await _eventPublisher.PublishOrderStatusChangedAsync(statusChangedEvent, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Статус заказа {OrderId} изменен с {PrevStatus} на {NewStatus}", order.Id, previousStatus, order.Status);
        }
    }

    /// <summary>
    /// Удаление заказа
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task DeleteOrderAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _repository.GetByIdAsync(id, cancellationToken);
        if (order == null)
        {
            throw new NotFoundException($"Заказ с идентификатором {id} не найден.");
        }

        await _repository.DeleteAsync(id, cancellationToken);
        _logger.LogInformation("Заказ {OrderId} ({OrderNumber}) удален", order.Id, order.OrderNumber);

        var deletedEvent = new OrderDeletedEvent
        {
            OrderId = order.Id,
            OrderNumber = order.OrderNumber
        };
        await _eventPublisher.PublishOrderDeletedAsync(deletedEvent, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
