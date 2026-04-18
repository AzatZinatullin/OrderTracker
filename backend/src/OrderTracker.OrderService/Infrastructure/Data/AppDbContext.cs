using Microsoft.EntityFrameworkCore;
using OrderTracker.OrderService.Domain.Entities;
using System.Reflection;
using MassTransit;

namespace OrderTracker.OrderService.Infrastructure.Data;

/// <summary>
/// Контекст базы данных для сервиса заказов.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="AppDbContext"/>.
    /// </summary>
    /// <param name="options">Опции контекста</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Набор сущностей заказов.
    /// </summary>
    public DbSet<Order> Orders { get; set; } = null!;

    /// <summary>
    /// Настраивает конфигурации моделей и сущности Outbox для обеспечения атомарности операций.
    /// </summary>
    /// <param name="modelBuilder">Построитель моделей для настройки сопоставления сущностей.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.AddTransactionalOutboxEntities();

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
