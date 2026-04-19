using OrderTracker.OrderService.Application.DTOs;
using OrderTracker.OrderService.Application.Validators;
using Xunit;

namespace OrderTracker.OrderService.Tests;

/// <summary>
/// Тесты для CreateOrderValidator
/// </summary>
public class CreateOrderValidatorTests
{
    private readonly CreateOrderValidator _validator = new();

    /// <summary>
    /// Проверяет валидацию с пустой строкой описания
    /// </summary>
    [Fact]
    public void Validate_WithEmptyDescription_ShouldHaveError()
    {
        // Arrange
        var request = new CreateOrderRequest("");
        
        // Act
        var result = _validator.Validate(request);
        
        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Description");
    }

    /// <summary>
    /// Проверяет валидацию с валидным описанием
    /// </summary>
    [Fact]
    public void Validate_WithValidDescription_ShouldNotHaveError()
    {
        // Arrange
        var request = new CreateOrderRequest("Valid desc");
        
        // Act
        var result = _validator.Validate(request);
        Assert.True(result.IsValid);
    }
}
