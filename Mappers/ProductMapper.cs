using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Dtos;
using DrinkConnect.Models;

namespace DrinkConnect.Mappers
{
    public static class ProductMapper
    {
        public static Product ToProductFromNewProductDto(this NewProductDto newProductDto){
            return new Product{
                Quantity = newProductDto.Quantity,
                Name = newProductDto.Name,
                Description = newProductDto.Description,
                Price = newProductDto.Price,
                Category = newProductDto.Category
            };
        }    

        public static Product EditProducFromDto(
        this Product product, EditProductDto dto){
            if (dto.Quantity.HasValue)
            {
                product.Quantity = dto.Quantity.Value;
            }

            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                product.Name = dto.Name;
            }

            if (!string.IsNullOrWhiteSpace(dto.Description))
            {
                product.Description = dto.Description;
            }

            if (dto.Price.HasValue)
            {
                product.Price = dto.Price.Value;
            }

            if (dto.Category.HasValue)
            {
                product.Category = dto.Category.Value;
            }

            return product;
        }
    }
}