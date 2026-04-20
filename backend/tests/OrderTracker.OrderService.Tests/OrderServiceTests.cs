using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using OrderTracker.OrderService.Application.DTOs;
using OrderTracker.OrderService.Application.Interfaces;
using OrderTracker.OrderService.Domain.Entities;
using OrderTracker.Shared.Enums;
using OrderTracker.Shared.Events;
using OrderTracker.Shared.Exceptions;

namespace OrderTracker.OrderService.Tests;

/// <summary>
/// Тесты для OrderService
/// </summary>
public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _repositoryMock;
    private readonly Mock<IEventPublisher> _publisherMock;
    private readonly Mock<ILogger<Application.Services.OrderService>> _loggerMock;
    private readonly Application.Services.OrderService _sut;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="OrderServiceTests"/>.
    /// </summary>
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

    /// <summary>
    /// Проверяет, что CreateOrderAsync создает заказ и публикует событие
    /// </summary>
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

    /// <summary>
    /// Проверяет, что UpdateOrderStatusAsync обновляет статус и публикует событие
    /// </summary>
    [Fact]
    public async Task UpdateOrderStatusAsync_ShouldUpdateAndPublishEvent_WhenChanged()
    {
        // Arrange
        var order = new Order("Test");
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

    /// <summary>
    /// Проверяет, что DeleteOrderAsync удаляет заказ и публикует событие
    /// </summary>
    [Fact]
    public async Task DeleteOrderAsync_ShouldDeleteAndPublishEvent_WhenOrderExists()
    {
        // Arrange
        var order = new Order("Тестовое описание");

        _repositoryMock
            .Setup(x => x.GetByIdAsync(order.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        // Act
        await _sut.DeleteOrderAsync(order.Id, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.DeleteAsync(order.Id, It.IsAny<CancellationToken>()), Times.Once);

        _publisherMock.Verify(x => x.PublishOrderDeletedAsync(
            It.Is<OrderDeletedEvent>(e =>
                e.OrderId == order.Id &&
                e.OrderNumber == order.OrderNumber),
            It.IsAny<CancellationToken>()), Times.Once);

        _repositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(order.Id.ToString())),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    /// <summary>
    /// Проверяет, что DeleteOrderAsync выбрасывает исключение при отсутствии заказа
    /// </summary>
    [Fact]
    public async Task DeleteOrderAsync_ShouldThrowNotFoundException_WhenOrderDoesNotExist()
    {
        // Arrange
        var randomId = Guid.NewGuid();
        _repositoryMock
            .Setup(x => x.GetByIdAsync(randomId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order?)null);

        // Act
        Func<Task> act = async () => await _sut.DeleteOrderAsync(randomId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"*{randomId}*");

        _repositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _publisherMock.Verify(x => x.PublishOrderDeletedAsync(It.IsAny<OrderDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
