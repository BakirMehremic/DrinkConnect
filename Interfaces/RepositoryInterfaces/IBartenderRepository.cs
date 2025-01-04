using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Enums;
using DrinkConnect.Models;

namespace DrinkConnect.Interfaces.RepositoryInterfaces
{
    public interface IBartenderRepository
    {
        Task<Notification?> GetNotificationById(int id);

        Task<Notification?> DeleteNotificationAsync(int id);

        Task<List<Order>?> GetOrdersAsync();   

        Task<Order?> UpdateOrderStatusAsync(int id, OrderStatus orderStatus);

        Task<Order?> DeleteOrderAsync(int id);

        Task<Order?> GetOrderByIdAsync(int id);

        Task<Product> AddProductAsync(Product product);

        Task<Product?> EditProductAsync(int id, Product product);

        Task<Product?> GetProductByIdAsync(int id);

        Task<Product?> DeleteProductAsync(int id);
    }
}