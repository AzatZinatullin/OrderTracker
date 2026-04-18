using OrderTracker.Shared.Enums;

namespace OrderTracker.OrderService.Domain.Entities;

/// <summary>
/// Сущность заказа. Содержит бизнес-логику (State Machine) для проверки переходов статусов.
/// </summary>
public class Order
{
    public Guid Id { get; private set; }
    public string OrderNumber { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public OrderStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    // Для EF Core
    protected Order() { }

    public Order(string orderNumber, string description)
    {
        Id = Guid.NewGuid();
        OrderNumber = !string.IsNullOrWhiteSpace(orderNumber) ? orderNumber : throw new ArgumentException(nameof(orderNumber));
        Description = description ?? string.Empty;
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
            throw new InvalidOperationException($"Cannot change status of a {Status} order.");
        }

        if (Status == OrderStatus.Created && newStatus == OrderStatus.Delivered)
        {
            throw new InvalidOperationException("Order cannot be delivered without being shipped first.");
        }

        if (Status == newStatus)
        {
            return; // No change
        }

        Status = newStatus;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
