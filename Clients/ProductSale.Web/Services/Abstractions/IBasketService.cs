using ProductSale.Web.Models.Basket;

namespace ProductSale.Web.Services.Abstractions
{
    public interface IBasketService
    {
        Task<bool> SaveOrUpdate(BasketViewModel basketViewModel);
        Task<BasketViewModel> Get();
        Task<bool?> Delete();

        Task AddBasketItem(BasketItemViewModel basketItemViewModel);
        Task<bool?> DeleteBasketItem(string courseId);

        Task<bool?> ApplyDiscount(string code);

        Task<bool?> CancelApplyDiscount();
    }
}
