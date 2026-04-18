using FluentValidation;
using OrderTracker.OrderService.Application.DTOs;

namespace OrderTracker.OrderService.Application.Validators;

/// <summary>
/// Валидатор создания заказа
/// </summary>
public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="CreateOrderValidator"/>.
    /// </summary>
    public CreateOrderValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Описание заказа обязательно.")
            .MaximumLength(1000).WithMessage("Описание заказа не должно превышать 1000 символов.");
    }
}
