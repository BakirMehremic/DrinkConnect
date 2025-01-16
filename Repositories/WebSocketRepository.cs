using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Data;
using DrinkConnect.Interfaces.RepositoryInterfaces;
using DrinkConnect.Models;

namespace DrinkConnect.Repositories
{
    public class WebSocketRepository : IWebSocketRepository
    {
        private readonly ApplicationDbContext _context;
        public WebSocketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Notification?> AddNotificationAsync(Notification notification){
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            return notification;
        }
    }
}