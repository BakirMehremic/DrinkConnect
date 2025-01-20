using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Enums;

namespace DrinkConnect.Dtos.UserDtos
{
    public class RegisterResponseDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}