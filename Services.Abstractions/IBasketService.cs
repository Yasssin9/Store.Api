using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.BasketDto;

namespace Services.Abstractions
{
    public interface IBasketService
    {
        Task<BasketDto> GetCustomerBasketAsync(string id);
        Task<BasketDto> UpdateBasketAsync(BasketDto basket);
        Task<bool> DeleteBasketAsync(string id);
    }
}
