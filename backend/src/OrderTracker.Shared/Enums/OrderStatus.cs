namespace OrderTracker.Shared.Enums;

/// <summary>
/// Статусы жизненного цикла заказа.
/// </summary>
public enum OrderStatus
{
    Created = 0,
    Shipped = 1,
    Delivered = 2,
    Cancelled = 3
}
