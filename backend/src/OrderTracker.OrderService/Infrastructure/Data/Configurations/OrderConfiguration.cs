using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderTracker.OrderService.Domain.Entities;
using OrderTracker.Shared.Constants;

namespace OrderTracker.OrderService.Infrastructure.Data.Configurations;

/// <summary>
/// Конфигурация сущности Order
/// </summary>
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    /// <summary>
    /// Настраивает конфигурацию сущности Order.
    /// </summary>
    /// <param name="builder">Построитель сущности Order.</param>
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrderNumber).IsRequired().HasMaxLength(OrderLimits.NumberMaxLength);
            
        builder.HasIndex(x => x.OrderNumber).IsUnique();

        builder.Property(x => x.Description).IsRequired().HasMaxLength(OrderLimits.DescriptionMaxLength);

        builder.Property(x => x.Status).HasConversion<string>().IsRequired().HasMaxLength(OrderLimits.StatusMaxLength);

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.Property(x => x.UpdatedAt).IsRequired();
    }
}
