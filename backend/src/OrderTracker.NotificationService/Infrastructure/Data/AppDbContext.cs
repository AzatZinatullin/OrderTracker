using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace OrderTracker.NotificationService.Infrastructure.Data;

/// <summary>
/// Контекст базы данных для сервиса уведомлений
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
    /// Настраивает конфигурации моделей, включая сущности Outbox для MassTransit.
    /// </summary>
    /// <param name="modelBuilder">Построитель моделей для настройки сопоставления сущностей с базой данных.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.AddTransactionalOutboxEntities();

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
