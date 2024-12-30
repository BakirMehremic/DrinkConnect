using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Dtos;
using DrinkConnect.Interfaces.RepositoryInterfaces;
using DrinkConnect.Interfaces.ServiceInterfaces;
using DrinkConnect.Mappers;
using DrinkConnect.Models;
using DrinkConnect.Utils;

namespace DrinkConnect.Services
{
    public class WaiterService : IWaiterService
    {
        private readonly IWaiterRepsoritoy _repository;
        private readonly UserUtils _userUtils;
        public WaiterService(IWaiterRepsoritoy repository, UserUtils userUtils)
        {
            _repository = repository;  
            _userUtils = userUtils;
        }

        public async Task<Order> AddOrderAsync(NewOrderDto newOrderDto)
        {
            var userId = _userUtils.GetCurrentUserId();
            if(userId is null)
                throw new Exception("Current user Id can not be found");
            
            var newOrder = OrderMapper.NewOrderDtoToOrder(newOrderDto, userId);

            return await _repository.AddOrderAsync(newOrder);
        }

        public async Task<Notification?> DeleteNotificationAsync(int id)
        {
            return await _repository.DeleteNotificationAsync(id);
        }

        public async Task<Order?> DeleteOrderAsync(int id)
        {
            return await _repository.DeleteOrderAsync(id);
        }

        public async Task<Order?> EditOrderAsync(int id, EditOrderDto editOrderDto)
        {
            //var product = _repository.Product.
            return null;
        }

        public Task<Notification?> GetNotificationById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>?> GetNotificationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order?> GetOrderByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>?> GetOrdersAsync()
        {
            throw new NotImplementedException();
        }
    }
}