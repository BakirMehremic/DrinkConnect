using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Dtos;
using DrinkConnect.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace DrinkConnect.Controllers
{
    [Route("bartender")]
    [ApiController]
    public class BartenderController : ControllerBase
    {
        private readonly IBartenderService _service;

        public BartenderController(IBartenderService service)
        {
            _service = service;
        }

        [HttpPost("add-product")]
        public async Task<IActionResult> AddProduct([FromBody] NewProductDto newProductDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var product = await _service.AddProductAsync(newProductDto);
            return Ok(product);
        }
    }
}