using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;
using StackExchange.Redis;

namespace Persistence.Repositories
{
    public class BasketRepository(IConnectionMultiplexer connection) : IBasketRepository
    {
        private readonly IDatabase _database = connection.GetDatabase();
        public async Task<bool> DeleteBasketAsync(string id)
        => await _database.KeyDeleteAsync(id);

        public async Task<CustomerBasket> GetCustomerBasketAsync(string id)
        {
            var basket=await _database.StringGetAsync(id);

            if (basket.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null)
        {
            var serializedBaslek = JsonSerializer.Serialize(basket);
            var isCreatedOrUpdated = await _database.StringSetAsync(basket.Id, serializedBaslek, timeToLive ?? TimeSpan.FromDays(30));
            return isCreatedOrUpdated ? await GetCustomerBasketAsync(basket.Id) : null;
        }
    }
}
