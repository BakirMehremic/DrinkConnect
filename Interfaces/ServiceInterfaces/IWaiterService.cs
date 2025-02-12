using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Dtos;
using DrinkConnect.Models;

namespace DrinkConnect.Interfaces.ServiceInterfaces
{
    public interface IWaiterService
    {

        Task<List<Order>?> GetOrdersAsync();

        Task<List<Notification>?> GetNotificationsAsync();

        Task<Order> AddOrderAsync(NewOrderDto newOrderDto);

        Task<Order?> EditOrderAsync(int id, EditOrderDto editOrderDto);

        Task<Order?> DeleteOrderAsync(int id);

        Task<Order?> GetOrderByIdAsync(int id);

        Task<Notification?> GetNotificationById(int id);

        Task<Notification?> DeleteNotificationAsync(int id);

        Task<Product?> GetProductByIdAsync(int id);

         
    }
}