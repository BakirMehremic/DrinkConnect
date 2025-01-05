using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Dtos;
using DrinkConnect.Interfaces.ServiceInterfaces;
using DrinkConnect.Models;
using DrinkConnect.Utils;

namespace DrinkConnect.Mappers
{
    public static class OrderMapper
    {
        public static async Task<Order> NewOrderDtoToOrderAsync(NewOrderDto dto, string userId, IWaiterService waiterService)
        {
            var orderProductsTasks = dto.OrderProducts.Select(async dtoProduct => 
            {
                var product = await waiterService.GetProductByIdAsync(dtoProduct.ProductId); 
                if (product == null)
                {
                    throw new Exception($"Product with ID {dtoProduct.ProductId} not found.");
                }
                
                return new OrderProduct
                {
                    ProductId = product.Id,
                    Product = product,
                    Quantity = dtoProduct.Quantity
                };
            }).ToList();

            var orderProducts = await Task.WhenAll(orderProductsTasks);

            float totalPrice = orderProducts.ToList().CalculateTotalPrice(); 

            return new Order
            {
                TotalPrice = totalPrice, 
                UserId = userId,
                OrderProducts = orderProducts.ToList() 
            };
        }

        

        public static async Task<Order> EditOrderDtoToOrderAsync(this Order order, EditOrderDto dto, IWaiterService waiterService)
        {

            if (dto.OrderProducts != null)
            {
                var orderProductsTasks = dto.OrderProducts.Select(async dtoProduct =>
                {
                    var product = await waiterService.GetProductByIdAsync(dtoProduct.ProductId);
                    if (product == null)
                    {
                        throw new Exception($"Product with ID {dtoProduct.ProductId} not found.");
                    }

                    return new OrderProduct
                    {
                        ProductId = product.Id,
                        Product = product,
                        Quantity = dtoProduct.Quantity
                    };
                }).ToList();

                var orderProducts = await Task.WhenAll(orderProductsTasks);
                
                order.OrderProducts = orderProducts.ToList();
                
                float totalPrice = orderProducts.ToList().CalculateTotalPrice(); 
                order.TotalPrice = totalPrice;

            }

            return order;
        }

    }
}