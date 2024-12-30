using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Dtos;
using DrinkConnect.Models;
using DrinkConnect.Utils;

namespace DrinkConnect.Mappers
{
    public static class OrderMapper
    {
        public static Order NewOrderDtoToOrder(NewOrderDto dto, string userId){
            return new Order{
                TotalPrice = dto.OrderProducts.CalculateTotalPrice(),
                UserId = userId,
                OrderProducts = dto.OrderProducts
            };
        }


        public static Order EditOrderDtoToOrder(this Order order, EditOrderDto dto){
            if(!string.IsNullOrWhiteSpace(dto.UserId))
                order.UserId = dto.UserId;
            if(dto.OrderProducts != null && dto.OrderProducts.Count > 0)
                order.OrderProducts = dto.OrderProducts;
            
            return order;
        }
    }
}