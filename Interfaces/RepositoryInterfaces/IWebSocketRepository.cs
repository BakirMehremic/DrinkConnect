using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Models;

namespace DrinkConnect.Interfaces.RepositoryInterfaces
{
    public interface IWebSocketRepository
    {
        Task<Notification?> AddNotificationAsync(Notification notification);
    }
}