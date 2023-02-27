using ProductSale.Service.Basket.Dtos;
using ProductSale.Shared.Infrastructure.Response.Base;

namespace ProductSale.Service.Basket.Services.Abstractions
{
    public interface IBasketService
    {
        Task<Response> Get(string userId);
        Task<Response> SaveOrUpdate(BasketDto basketDto);
        Task<Response> Delete(string userId);
    }
}
