using OrderTracker.OrderService.Domain.Entities;
using OrderTracker.Shared.Constants;
using OrderTracker.Shared.Enums;
using OrderTracker.Shared.Exceptions;
using FluentAssertions;

namespace OrderTracker.OrderService.Tests
{
    /// <summary>
    /// Тесты для сущности Order
    /// </summary>
    public class OrderTests
    {
        /// <summary>
        /// Проверяет, что конструктор выбрасывает исключение при слишком длинном описании
        /// </summary>
        [Fact]
        public void Constructor_ShouldThrowException_WhenDescriptionIsTooLong()
        {
            // Arrange
            var longDescription = new string('a', OrderLimits.DescriptionMaxLength + 10);

            // Act
            Action act = () => new Order(longDescription);

            // Assert
            act.Should().Throw<DomainException>().WithMessage("Описание заказа должно быть от*");
        }

        /// <summary>
        /// Проверяет, что конструктор выбрасывает исключение при пустом описании
        /// </summary>
        [Fact]
        public void Constructor_ShouldThrowException_WhenDescriptionIsEmpty()
        {
            // Arrange
            var emptyDescription = "   ";

            // Act
            Action act = () => new Order(emptyDescription);

            // Assert
            act.Should().Throw<DomainException>().WithMessage("Описание заказа должно быть от*");
        }

        /// <summary>
        /// Проверяет, что конструктор выбрасывает исключение при неверном переходе статуса
        /// </summary>
        [Fact]
        public void UpdateStatus_ShouldThrowException_WhenTransitionIsInvalid()
        {
            // Arrange
            var order = new Order("Описание");

            // Act
            Action act = () => order.UpdateStatus(OrderStatus.Delivered);

            // Assert
            act.Should().Throw<DomainException>().WithMessage("*без предварительной отправки*");
        }
    }
}
