using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OrderTracker.OrderService.Application.DTOs;
using OrderTracker.OrderService.Application.Interfaces;

namespace OrderTracker.OrderService.Controllers;

/// <summary>
/// Контроллер заказов
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="OrdersController"/>.
    /// </summary>
    /// <param name="orderService">Сервис заказов</param>
    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Создание нового заказа
    /// </summary>
    /// <param name="validator">`CreateOrderRequest` валидатор</param>
    /// <param name="request">Запрос на создание заказа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат операции: 201 (успех) и созданный заказ или 400 (неверный запрос).</returns>
    [HttpPost]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder(IValidator<CreateOrderRequest> validator, [FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            // Для простоты и скорости реализации ошибки в виде строки.
            return BadRequest(string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage)));
        }

        var response = await _orderService.CreateOrderAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetOrderById), new { id = response.Id }, response);
    }

    /// <summary>
    /// Получение всех заказов
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат операции: 200 (успех) и список заказов.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllOrders(CancellationToken cancellationToken)
    {
        var response = await _orderService.GetAllOrdersAsync(cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Получение заказа по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат операции: 200 (успех) или 404 (заказ не найден).</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderById(Guid id, CancellationToken cancellationToken)
    {
        var response = await _orderService.GetOrderByIdAsync(id, cancellationToken);
        
        if (response == null)
            return NotFound();

        return Ok(response);
    }

    /// <summary>
    /// Обновление статуса заказа
    /// </summary>
    /// <param name="validator">`UpdateOrderStatusRequest` валидатор</param>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="request">Запрос на обновление статуса</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат операции: 200 (успех), 400 (неверный статус) или 404 (заказ не найден).</returns>
    [HttpPut("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOrderStatus(IValidator<UpdateOrderStatusRequest> validator, Guid id, [FromBody] UpdateOrderStatusRequest request, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            // Для простоты и скорости реализации ошибки в виде строки.
            return BadRequest(string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage)));
        }

        await _orderService.UpdateOrderStatusAsync(id, request, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Удаление заказа
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат операции: 204 (успех) или 404 (заказ не найден).</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOrder(Guid id, CancellationToken cancellationToken)
    {
        await _orderService.DeleteOrderAsync(id, cancellationToken);
        return Ok();
    }
}
