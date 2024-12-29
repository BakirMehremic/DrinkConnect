using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Iana;

namespace DrinkConnect.Controllers
{
    [ApiController]
    public class TestController : ControllerBase
    {
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("testadmin")]
        public IActionResult test(){
            return Ok();
        }

        [Authorize(Policy = "WaiterOnly")]
        [HttpGet("testwaiter")]
        public IActionResult testwaiter(){
            return Ok();
        }

        [Authorize(Policy = "BartenderOnly")]
        [HttpGet("testbartender")]
        public IActionResult testbartender(){
            return Ok();
        }
        
    }
}