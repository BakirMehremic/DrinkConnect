using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Models;
using DrinkConnect.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DrinkConnect.Dtos
{
    public class NewOrderDto
    {
        [Required]
        public required ICollection<NewOrderProductDto> OrderProducts {get; set;}
    }
}