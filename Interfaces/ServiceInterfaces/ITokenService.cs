using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Models;

namespace DrinkConnect.Interfaces.ServiceInterfaces
{
    public interface ITokenService
    {
        string CreateRefreshToken();
        string CreateToken(User user);
        
    }
}