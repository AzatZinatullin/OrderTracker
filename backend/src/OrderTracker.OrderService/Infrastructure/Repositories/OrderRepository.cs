using Microsoft.EntityFrameworkCore;
using OrderTracker.OrderService.Application.Interfaces;
using OrderTracker.OrderService.Domain.Entities;
using OrderTracker.OrderService.Infrastructure.Data;

namespace OrderTracker.OrderService.Infrastructure.Repositories;

/// <summary>
/// Репозиторий заказов
/// </summary>
public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="OrderRepository"/>.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Добавление заказа
    /// </summary>
    /// <param name="order">Заказ</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _context.Orders.AddAsync(order, cancellationToken);
    }

    /// <summary>
    /// Получение всех заказов
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Список заказов</returns>
    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Получение заказа по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Заказ</returns>
    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Orders.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <summary>
    /// Обновление заказа
    /// </summary>
    /// <param name="order">Заказ</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        _context.Orders.Update(order);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Удаление заказа
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await GetByIdAsync(id, cancellationToken);
        if (order != null)
        {
            _context.Orders.Remove(order);
        }
    }

    /// <summary>
    /// Сохранение изменений
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
