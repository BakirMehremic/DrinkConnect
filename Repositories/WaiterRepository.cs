using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Data;
using DrinkConnect.Interfaces.RepositoryInterfaces;
using DrinkConnect.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DrinkConnect.Repositories
{
    public class WaiterRepository : IWaiterRepsoritoy
    {
        private readonly ApplicationDbContext _context;
        public WaiterRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Order> AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Notification?> DeleteNotificationAsync(int id)
        {
            var notification = await _context.Notifications.
            FirstOrDefaultAsync(x => x.Id == id);

            if(notification is null) return null;

            _context.Remove(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<Order?> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.
            FirstOrDefaultAsync(x => x.Id == id);

            if(order is null) return null;

            _context.Remove(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> EditOrderAsync(int id, Order order)
        {
            var oldOrder = await _context.Orders.
            FirstOrDefaultAsync(x => x.Id == id);

            if(oldOrder is null) return null;
            oldOrder.OrderProducts = order.OrderProducts;
            oldOrder.Status = order.Status;
            oldOrder.TotalPrice = order.TotalPrice;
            oldOrder.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Notification?> GetNotificationById(int id)
        {
            return await _context.Notifications.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            // include makes sure that list of related orderproducts is returned 
            return await _context.Orders.Include(o => o.OrderProducts)
            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Order>?> GetOrdersAsync()
        {
            return await _context.Orders.Include(o => o.OrderProducts)
            .ToListAsync();
        }

        public async Task<List<Notification>?> GetNotificationsAsync(){
            return await _context.Notifications.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}