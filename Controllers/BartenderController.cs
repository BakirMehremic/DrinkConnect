using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Dtos;
using DrinkConnect.Enums;
using DrinkConnect.Interfaces.ServiceInterfaces;
using DrinkConnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrinkConnect.Controllers
{
    [Authorize(Policy = "BartenderOnly")]
    [Route("bartender")]
    [ApiController]
    public class BartenderController : ControllerBase
    {
        private readonly IBartenderService _service;
        private readonly IWebSocketService _webSocketService;

        public BartenderController(IBartenderService service, IWebSocketService webSocketService)
        {
            _service = service;
            _webSocketService = webSocketService;
        }


        [HttpPost("product")]
        public async Task<IActionResult> AddProduct([FromBody] NewProductDto newProductDto)
        {
            if (newProductDto == null)
                return BadRequest("Invalid product data.");

            var product = await _service.AddProductAsync(newProductDto);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut("product/{id}")]
        public async Task<IActionResult> EditProduct(int id, [FromBody] EditProductDto editProductDto)
        {
            if (editProductDto == null)
                return BadRequest("Invalid product data.");

            var updatedProduct = await _service.EditProductAsync(id, editProductDto);
            if (updatedProduct == null)
                return NotFound($"Product with ID {id} not found.");

            return Ok(updatedProduct);
        }

        [HttpDelete("product/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _service.DeleteProductAsync(id);
            if (product == null)
                return NotFound($"Product with ID {id} not found.");

            return Ok(product);
        }

        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _service.GetProductByIdAsync(id);
            if (product == null)
                return NotFound($"Product with ID {id} not found.");

            return Ok(product);
        }

        [HttpPut("order/{id}/status")] 
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromQuery] OrderStatus orderStatus)
        {
            var updatedOrder = await _service.UpdateOrderStatusAsync(id, orderStatus);
            if (updatedOrder == null)
                return NotFound($"Order with ID {id} not found.");

            string message = $"Order with id {id} was updated to {updatedOrder.Status}";

            await _webSocketService.SendNotificationAsync(updatedOrder, message);

            return Ok(updatedOrder);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _service.GetOrdersAsync();
            if (orders == null || orders.Count == 0)
                return NotFound("No orders found.");

            return Ok(orders);
        }

        [HttpGet("order/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _service.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound($"Order with ID {id} not found.");

            return Ok(order);
        }

        [HttpDelete("order/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _service.DeleteOrderAsync(id);
            if (order == null)
                return NotFound($"Order with ID {id} not found.");

            return Ok(order);
        }

        [HttpGet("notification/{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var notification = await _service.GetNotificationById(id);
            if (notification == null)
                return NotFound($"Notification with ID {id} not found.");

            return Ok(notification);
        }

        [HttpDelete("notification/{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var notification = await _service.DeleteNotificationAsync(id);
            if (notification == null)
                return NotFound($"Notification with ID {id} not found.");

            return Ok(notification);
        }
    }
}