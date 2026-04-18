using OrderTracker.OrderService.Application.DTOs;

namespace OrderTracker.OrderService.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса заказов
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Создание нового заказа
    /// </summary>
    /// <param name="request">Запрос на создание заказа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Информация о созданном заказ</returns>
    Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение заказа по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Информация о заказе</returns>
    Task<OrderResponse?> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение всех заказов
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список заказов</returns>
    Task<IEnumerable<OrderResponse>> GetAllOrdersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновление статуса заказа
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="request">Запрос на обновление статуса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task UpdateOrderStatusAsync(Guid id, UpdateOrderStatusRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление заказа
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DeleteOrderAsync(Guid id, CancellationToken cancellationToken = default);
}
