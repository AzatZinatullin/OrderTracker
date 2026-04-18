using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderTracker.OrderService.Application.DTOs;
using OrderTracker.OrderService.Application.Interfaces;
using OrderTracker.OrderService.Controllers;
using OrderTracker.Shared.Enums;
using Xunit;

namespace OrderTracker.OrderService.Tests;

public class OrdersControllerTests
{
    private readonly Mock<IOrderService> _orderServiceMock;
    private readonly OrdersController _controller;

    public OrdersControllerTests()
    {
        _orderServiceMock = new Mock<IOrderService>();
        _controller = new OrdersController(_orderServiceMock.Object);
    }

    [Fact]
    public async Task CreateOrder_ReturnsCreatedAtAction()
    {
        var request = new CreateOrderRequest("Test");
        var response = new OrderResponse(Guid.NewGuid(), "ORD-1", "Test", OrderStatus.Created, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);

        _orderServiceMock.Setup(x => x.CreateOrderAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(response);

        var result = await _controller.CreateOrder(request, CancellationToken.None);

        var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("GetOrderById", createdAtResult.ActionName);
        Assert.Equal(response, createdAtResult.Value);
    }
}
