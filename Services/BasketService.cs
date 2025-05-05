using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Services.Abstractions;
using Shared.BasketDto;

namespace Services
{
    public class BasketService(IMapper mapper,IBasketRepository basketRepository) : IBasketService
    {
        public async Task<bool> DeleteBasketAsync(string id)
        => await basketRepository.DeleteBasketAsync(id);

        public async Task<BasketDto> GetCustomerBasketAsync(string id)
        {
            var basket = await basketRepository.GetCustomerBasketAsync(id);
            return basket is null ? throw new BasketNotFoundException(id) 
                                  : mapper.Map<BasketDto>(basket);
        }

        public async Task<BasketDto> UpdateBasketAsync(BasketDto basket)
        {
            var customerBasket = mapper.Map<CustomerBasket>(basket);
            
            var updatedBasket= await basketRepository.UpdateBasketAsync(customerBasket);

            return updatedBasket is null ? throw new Exception($"Can't Update Basket Now!")
                                         : mapper.Map<BasketDto>(updatedBasket);
        }

       
    }
}
