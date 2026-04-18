using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using OrderTracker.NotificationService.Consumers;
using OrderTracker.NotificationService.Hubs;
using OrderTracker.Shared.Enums;
using OrderTracker.Shared.Events;
using Xunit;

namespace OrderTracker.NotificationService.Tests;

public class OrderStatusChangedConsumerTests
{
    [Fact]
    public async Task Consume_ShouldBroadcastToSignalR()
    {
        // Arrange
        var hubContextMock = new Mock<IHubContext<OrderTrackingHub>>();
        var clientsMock = new Mock<IHubClients>();
        var clientProxyMock = new Mock<IClientProxy>();
        var loggerMock = new Mock<ILogger<OrderStatusChangedConsumer>>();

        clientsMock.Setup(c => c.All).Returns(clientProxyMock.Object);
        clientsMock.Setup(c => c.Group(It.IsAny<string>())).Returns(clientProxyMock.Object);
        hubContextMock.Setup(h => h.Clients).Returns(clientsMock.Object);

        var consumer = new OrderStatusChangedConsumer(hubContextMock.Object, loggerMock.Object);
        var consumeContextMock = new Mock<ConsumeContext<OrderStatusChangedEvent>>();
        
        var @event = new OrderStatusChangedEvent
        {
            OrderId = Guid.NewGuid(),
            OrderNumber = "ORD-TEST",
            PreviousStatus = OrderStatus.Created,
            NewStatus = OrderStatus.Shipped,
            UpdatedAt = DateTimeOffset.UtcNow
        };
        consumeContextMock.Setup(x => x.Message).Returns(@event);

        // Act
        await consumer.Consume(consumeContextMock.Object);

        // Assert
        clientProxyMock.Verify(x => x.SendCoreAsync("OrderStatusUpdated", new object[] { @event }, default), Times.Once);
        clientProxyMock.Verify(x => x.SendCoreAsync("OrderStatusUpdatedAll", new object[] { @event }, default), Times.Once);
    }
}
