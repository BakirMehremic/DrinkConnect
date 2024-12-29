using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Dtos;
using DrinkConnect.Enums;
using DrinkConnect.Interfaces.RepositoryInterfaces;
using DrinkConnect.Interfaces.ServiceInterfaces;
using DrinkConnect.Models;

namespace DrinkConnect.Services
{
    public class BartenderService : IBartenderService
    {
        private readonly IBartenderRepository _repository;

        public BartenderService(IBartenderRepository repository)
        {
            _repository = repository;
        }


        public async Task<Product> AddProductAsync(NewProductDto newProductDto)
        {
            throw new NotImplementedException();
        }

        public Task<Notification?> DeleteNotificationAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Order?> DeleteOrderAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product?> DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Product?> EditProductAsync(EditProductDto editProductDto)
        {
            throw new NotImplementedException();
        }

        public Task<Notification?> GetNotificationById(int id)
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

        public Task<Product?> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Order?> UpdateOrderStatusAsync(int id, OrderStatus orderStatus)
        {
            throw new NotImplementedException();
        }
    }
}