using FluentValidation;
using OrderTracker.OrderService.Application.DTOs;

namespace OrderTracker.OrderService.Application.Validators;

/// <summary>
/// Валидатор обновления статуса заказа
/// </summary>
public class UpdateStatusValidator : AbstractValidator<UpdateOrderStatusRequest>
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UpdateStatusValidator"/>.
    /// </summary>
    public UpdateStatusValidator()
    {
        RuleFor(x => x.NewStatus)
            .IsInEnum().WithMessage("Недопустимый статус заказа.");
    }
}
