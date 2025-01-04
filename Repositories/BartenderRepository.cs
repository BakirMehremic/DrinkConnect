using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Data;
using DrinkConnect.Enums;
using DrinkConnect.Interfaces.RepositoryInterfaces;
using DrinkConnect.Models;
using Microsoft.EntityFrameworkCore;


namespace DrinkConnect.Repositories
{
    public class BartenderRepository : IBartenderRepository
    {
        private readonly ApplicationDbContext _context;

        public BartenderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
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

        public async Task<Product?> DeleteProductAsync(int id)
        {
            var product = await _context.Products.
            FirstOrDefaultAsync(x => x.Id == id);

            if(product is null) return null;

            _context.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> EditProductAsync(int id, Product product)
        {
            var toEdit = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id);

            if (toEdit is null) return null;

            toEdit.Quantity = product.Quantity;
            toEdit.Name = product.Name;
            toEdit.Description = product.Description;
            toEdit.Price = product.Price;
            toEdit.Category = product.Category;

            _context.Products.Update(toEdit);
            await _context.SaveChangesAsync();
            return toEdit;
        }


        public async Task<Notification?> GetNotificationById(int id)
        {
            return await _context.Notifications.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Order>?> GetOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Order?> UpdateOrderStatusAsync(int id, OrderStatus orderStatus)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);

            if (order is null) return null;

            order.Status = orderStatus;

            await _context.SaveChangesAsync();
            return order;
        }

    }
}