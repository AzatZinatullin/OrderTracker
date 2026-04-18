using OrderTracker.OrderService.Application.DTOs;
using OrderTracker.OrderService.Application.Validators;
using Xunit;

namespace OrderTracker.OrderService.Tests;

public class CreateOrderValidatorTests
{
    private readonly CreateOrderValidator _validator = new();

    [Fact]
    public void Validate_WithEmptyDescription_ShouldHaveError()
    {
        var request = new CreateOrderRequest("");
        var result = _validator.Validate(request);
        
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Description");
    }

    [Fact]
    public void Validate_WithValidDescription_ShouldNotHaveError()
    {
        var request = new CreateOrderRequest("Valid desc");
        var result = _validator.Validate(request);
        
        Assert.True(result.IsValid);
    }
}
