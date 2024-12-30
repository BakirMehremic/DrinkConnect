using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Dtos;
using DrinkConnect.Enums;
using DrinkConnect.Interfaces.RepositoryInterfaces;
using DrinkConnect.Interfaces.ServiceInterfaces;
using DrinkConnect.Mappers;
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
            var product = newProductDto.ToProductFromNewProductDto();
            return await _repository.AddProductAsync(product);
        }

        public async Task<Notification?> DeleteNotificationAsync(int id)
        {
            return await _repository.DeleteNotificationAsync(id);
        }

        public async Task<Order?> DeleteOrderAsync(int id)
        {
            return await _repository.DeleteOrderAsync(id);
        }

        public async Task<Product?> DeleteProductAsync(int id)
        {
            return await _repository.DeleteProductAsync(id);
        }

        public async Task<Product?> EditProductAsync(int id, EditProductDto dto)
        {
            
            var product = await _repository.GetProductByIdAsync(id);
            if (product is null) return null;

            var updated = product.EditProducFromDto(dto); 

            return await _repository.EditProductAsync(updated);
        }

        public async Task<Notification?> GetNotificationById(int id)
        {
            return await _repository.GetNotificationById(id);
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _repository.GetOrderByIdAsync(id);
        }

        public async Task<List<Order>?> GetOrdersAsync()
        {
            return await _repository.GetOrdersAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _repository.GetProductByIdAsync(id);
        }

        public async Task<Order?> UpdateOrderStatusAsync(int id, OrderStatus orderStatus)
        {
            return await _repository.UpdateOrderStatusAsync(id, orderStatus);
        }
    }
}