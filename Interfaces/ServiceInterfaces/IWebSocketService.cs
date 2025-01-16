using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Dtos.CRUDDtos;
using DrinkConnect.Models;

namespace DrinkConnect.Interfaces.ServiceInterfaces
{
    public interface IWebsocketService
    {
        Task NotifyWaiterAsync(string UserId, int OrderId, string Message);
    }
}