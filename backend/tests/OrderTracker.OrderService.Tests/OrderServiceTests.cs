using Microsoft.Extensions.Logging;
using Moq;
using OrderTracker.OrderService.Application.DTOs;
using OrderTracker.OrderService.Application.Interfaces;
using OrderTracker.OrderService.Domain.Entities;
using OrderTracker.Shared.Enums;
using OrderTracker.Shared.Events;

namespace OrderTracker.OrderService.Tests;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _repositoryMock;
    private readonly Mock<IEventPublisher> _publisherMock;
    private readonly Mock<ILogger<Application.Services.OrderService>> _loggerMock;
    private readonly Application.Services.OrderService _sut;

    public OrderServiceTests()
    {
        _repositoryMock = new Mock<IOrderRepository>();
        _publisherMock = new Mock<IEventPublisher>();
        _loggerMock = new Mock<ILogger<Application.Services.OrderService>>();
        
        _sut = new Application.Services.OrderService(
            _repositoryMock.Object,
            _publisherMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task CreateOrderAsync_ShouldCreateAndPublishEvent()
    {
        // Arrange
        var request = new CreateOrderRequest("Test order");

        // Act
        var response = await _sut.CreateOrderAsync(request, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("Test order", response.Description);
        Assert.Equal(OrderStatus.Created, response.Status);

        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
        _publisherMock.Verify(x => x.PublishOrderCreatedAsync(It.IsAny<OrderCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateOrderStatusAsync_ShouldUpdateAndPublishEvent_WhenChanged()
    {
        // Arrange
        var order = new Order("ORD-123", "Test");
        _repositoryMock.Setup(x => x.GetByIdAsync(order.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);
            
        var request = new UpdateOrderStatusRequest(OrderStatus.Shipped);

        // Act
        await _sut.UpdateOrderStatusAsync(order.Id, request, CancellationToken.None);

        // Assert
        Assert.Equal(OrderStatus.Shipped, order.Status);
        _repositoryMock.Verify(x => x.UpdateAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        _publisherMock.Verify(x => x.PublishOrderStatusChangedAsync(
            It.Is<OrderStatusChangedEvent>(e => e.NewStatus == OrderStatus.Shipped), 
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
