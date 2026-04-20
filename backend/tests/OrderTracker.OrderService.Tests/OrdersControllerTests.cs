using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderTracker.OrderService.Application.DTOs;
using OrderTracker.OrderService.Application.Interfaces;
using OrderTracker.OrderService.Application.Validators;
using OrderTracker.OrderService.Controllers;
using OrderTracker.Shared.Constants;
using OrderTracker.Shared.Enums;

namespace OrderTracker.OrderService.Tests;

/// <summary>
/// Тесты для OrdersController
/// </summary>
public class OrdersControllerTests
{
    private readonly Mock<IOrderService> _orderServiceMock;
    private readonly OrdersController _controller;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="OrdersControllerTests"/>.
    /// </summary>
    public OrdersControllerTests()
    {
        _orderServiceMock = new Mock<IOrderService>();
        _controller = new OrdersController(_orderServiceMock.Object);
    }

    /// <summary>
    /// Проверяет, что CreateOrder возвращает CreatedAtActionResult
    /// </summary>
    [Fact]
    public async Task CreateOrder_ReturnsCreatedAtAction()
    {
        // Arrange
        var validator = new CreateOrderValidator();
        var request = new CreateOrderRequest("Test");
        var response = new OrderResponse(Guid.NewGuid(), "ORD-1", "Test", OrderStatus.Created, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);
        _orderServiceMock.Setup(x => x.CreateOrderAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act
        var result = await _controller.CreateOrder(validator, request, CancellationToken.None);

        // Assert
        var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("GetOrderById", createdAtResult.ActionName);
        Assert.Equal(response, createdAtResult.Value);
    }

    /// <summary>
    /// Проверяет, что CreateOrder возвращает BadRequest при слишком длинном описании
    /// </summary>
    [Fact]
    public async Task CreateOrder_ReturnsBadRequest_WhenDescriptionIsTooLong()
    {
        // Arrange
        var longDescription = new string('A', OrderLimits.DescriptionMaxLength + 10);
        var request = new CreateOrderRequest(longDescription);
        var validator = new CreateOrderValidator();

        // Act
        var result = await _controller.CreateOrder(validator, request, CancellationToken.None);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

        // Assert
        Assert.NotNull(badRequestResult.Value);
        _orderServiceMock.Verify(x => x.CreateOrderAsync(It.IsAny<CreateOrderRequest>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
