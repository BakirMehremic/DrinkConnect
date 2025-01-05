using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Models;

namespace DrinkConnect.Utils
{
    public static class OrderUtils
    {
        public static float CalculateTotalPrice(this ICollection<OrderProduct> orderProducts){
            float sum = 0;
            foreach(OrderProduct op in orderProducts){
                sum+=op.Product.Price*op.Quantity;
            }
            return sum;
        }   

        public static bool CheckProductsAvaliability(this Order order){
            foreach(OrderProduct op in order.OrderProducts){
                if(op.Product.Quantity<op.Quantity)
                    return false;
            }
            return true;
        }

        public static void DecreaseProductsQuantity(this Order order){
            foreach(OrderProduct op in order.OrderProducts){
                op.Product.Quantity-=op.Quantity;
            }
        }
    }
}