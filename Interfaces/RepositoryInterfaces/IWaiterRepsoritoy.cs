
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Models;

namespace DrinkConnect.Interfaces.RepositoryInterfaces
{
    public interface IWaiterRepsoritoy
    {
        Task<List<Order>?> GetOrdersAsync();

        Task<List<Notification>?> GetNotificationsAsync();

        Task<Order> AddOrderAsync(Order order);

        Task<Order?> EditOrderAsync(int id, Order order);

        Task<Order?> DeleteOrderAsync(int id);

        Task<Order?> GetOrderByIdAsync(int id);

        Task<Notification?> GetNotificationById(int id);

        Task<Notification?> DeleteNotificationAsync(int id);

    }
}