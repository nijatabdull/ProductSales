using ProductSale.Web.Models.Discount;

namespace ProductSale.Web.Services.Abstractions
{
    public interface IDiscountService
    {
        Task<DiscountViewModel> GetDiscountByCode(string code);
    }
}
