using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Dtos;
using DrinkConnect.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrinkConnect.Controllers
{   
    [Authorize(Policy = "WaiterOnly")]
    [Route("bartender")]
    [ApiController]
    public class WaiterController : ControllerBase
    {
        public readonly IWaiterService _service;

        public WaiterController(IWaiterService service)
        {
            _service = service;
        }
        
        [HttpPost("order")]
        public async Task<IActionResult> AddOrder([FromBody] NewOrderDto newOrderDto)
        {
            if (newOrderDto == null)
                return BadRequest("Invalid order data.");

            try
            {
                var order = await _service.AddOrderAsync(newOrderDto);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("order/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _service.DeleteOrderAsync(id);
            if (order == null)
                return NotFound($"Order with ID {id} not found.");

            return Ok(order);
        }


        [HttpPut("order/{id}")]
        public async Task<IActionResult> EditOrder(int id, [FromBody] EditOrderDto editOrderDto)
        {
            if (editOrderDto == null)
                return BadRequest("Invalid order data.");

            var updatedOrder = await _service.EditOrderAsync(id, editOrderDto);
            if (updatedOrder == null)
                return NotFound($"Order with ID {id} not found.");

            return Ok(updatedOrder);
        }

        [HttpGet("order/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _service.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound($"Order with ID {id} not found.");

            return Ok(order);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _service.GetOrdersAsync();
            if (orders == null || orders.Count == 0)
                return NotFound("No orders found.");

            return Ok(orders);
        }
    }
}