using ProductSale.Shared.Infrastructure.Response;
using ProductSale.Web.Models.Discount;
using ProductSale.Web.Services.Abstractions;

namespace ProductSale.Web.Services.Concretes
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;

        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DiscountViewModel> GetDiscountByCode(string code)
        {
            var response = await _httpClient.GetAsync("discount/getbycode?code=" + code);

            if (response.IsSuccessStatusCode is false) return default;

            SuccessResponse<DiscountViewModel> discountViewModel = await response.Content.ReadFromJsonAsync<SuccessResponse<DiscountViewModel>>();

            return discountViewModel.Data;
        }
    }
}
