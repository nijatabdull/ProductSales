using ProductSale.Shared.Infrastructure.Response.Base;

namespace ProductSale.Service.Discount.Services.Abstractions
{
    public interface IDiscountService
    {
        Task<Response> All();
        Task<Response> Create(Models.Discount discount);
        Task<Response> GetById(int id);
        Task<Response> Update(Models.Discount discount);
        Task<Response> Delete(int id);
        Task<Response> GetByCode(string code, string userId);
    }
}
