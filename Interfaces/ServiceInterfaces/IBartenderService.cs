using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Dtos;
using DrinkConnect.Enums;
using DrinkConnect.Models;

namespace DrinkConnect.Interfaces.ServiceInterfaces
{
    public interface IBartenderService
    {
        Task<Notification?> GetNotificationById(int id);

        Task<Notification?> DeleteNotificationAsync(int id);

        Task<List<Order>?> GetOrdersAsync();   

        Task<Order?> UpdateOrderStatusAsync(int id, OrderStatus orderStatus);

        Task<Order?> DeleteOrderAsync(int id);

        Task<Order?> GetOrderByIdAsync(int id);

        Task<Product> AddProductAsync(NewProductDto newProductDto);

        Task<Product?> EditProductAsync(int id, EditProductDto editProductDto);

        Task<Product?> GetProductByIdAsync(int id);

        Task<Product?> DeleteProductAsync(int id);
        
    }

}