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
        var maxLength = 1000;
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Описание заказа обязательно.")
            .MaximumLength(maxLength).WithMessage($"Описание заказа не должно превышать {maxLength} символов.");
    }
}
