using ProductSale.Service.Basket.Dtos;
using ProductSale.Service.Basket.Services.Abstractions;
using ProductSale.Shared.Infrastructure.Response;
using ProductSale.Shared.Infrastructure.Response.Base;
using StackExchange.Redis;
using System.Text.Json;

namespace ProductSale.Service.Basket.Services.Concretes
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task<Response> Delete(string userId)
        {
           bool isDeleted = await _redisService.GetDb().KeyDeleteAsync(userId);

            if (isDeleted)
                return new SuccessResponse<string>();

            return new ErrorResponse("Melumat silinmedi");
        }

        public async Task<Response> Get(string userId)
        {
            RedisValue redisValue = await _redisService.GetDb().StringGetAsync(userId);

            if (string.IsNullOrEmpty(redisValue))
                return new ErrorResponse("asd");

            return new SuccessResponse<BasketDto>(JsonSerializer.Deserialize<BasketDto>(redisValue));
        }

        public async Task<Response> SaveOrUpdate(BasketDto basketDto)
        {
            bool isAdded = await _redisService.GetDb().StringSetAsync(basketDto.UserId, JsonSerializer.Serialize(basketDto));

            if (isAdded)
                return new SuccessResponse<BasketDto>(basketDto);

            return new ErrorResponse("Melumat daxil edilmedi");
        }
    }
}
