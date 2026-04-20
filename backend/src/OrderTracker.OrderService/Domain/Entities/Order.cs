using OrderTracker.Shared.Constants;
using OrderTracker.Shared.Enums;
using OrderTracker.Shared.Exceptions;

namespace OrderTracker.OrderService.Domain.Entities;

/// <summary>
/// Сущность заказа. Содержит бизнес-логику (State Machine) для проверки переходов статусов.
/// </summary>
public class Order
{
    /// <summary>
    /// Идентификатор заказа.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Номер заказа.
    /// </summary>
    public string OrderNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Описание заказа.
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Статус заказа.
    /// </summary>
    public OrderStatus Status { get; private set; }

    /// <summary>
    /// Дата и время создания заказа.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Дата и время последнего обновления заказа.
    /// </summary>
    public DateTimeOffset UpdatedAt { get; private set; }

    // Для EF Core
    protected Order() { }

    /// <summary>
    /// Создает новый экземпляр класса <see cref="Order" />.
    /// </summary>
    /// <param name="description">Описание заказа.</param>
    public Order(string description)
    {
        var orderNumber = GenerateOrderNumber();

        if (orderNumber.Length > OrderLimits.NumberMaxLength)
        {
            throw new DomainException($"Номер заказа не должен превышать {OrderLimits.NumberMaxLength}");
        }

        if (string.IsNullOrWhiteSpace(description) 
            || description.Length < OrderLimits.DescriptionMinLength
            || description.Length > OrderLimits.DescriptionMaxLength)
        {
            throw new DomainException($"Описание заказа должно быть от {OrderLimits.DescriptionMinLength} до {OrderLimits.DescriptionMaxLength} символов");
        }

        Id = Guid.NewGuid();
        OrderNumber = orderNumber;
        Description = description;
        Status = OrderStatus.Created;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Обновление статуса заказа с проверкой валидности перехода.
    /// </summary>
    public void UpdateStatus(OrderStatus newStatus)
    {
        if (Status == OrderStatus.Delivered || Status == OrderStatus.Cancelled)
        {
            throw new DomainException($"Нельзя изменить статус у {Status} заказа.");
        }

        if (Status == OrderStatus.Created && newStatus == OrderStatus.Delivered)
        {
            throw new DomainException("Нельзя доставить заказ без предварительной отправки.");
        }

        if (Status == newStatus)
        {
            return;
        }

        Status = newStatus;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Генерация номера заказа.
    /// </summary>
    private static string GenerateOrderNumber() => $"ORD-{DateTimeOffset.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
}
