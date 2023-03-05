using ProductSale.Shared.Infrastructure.Response;
using ProductSale.Web.Models.Basket;
using ProductSale.Web.Models.Discount;
using ProductSale.Web.Services.Abstractions;

namespace ProductSale.Web.Services.Concretes
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;
        private readonly IDiscountService _discountService;

        public BasketService(HttpClient httpClient, IDiscountService discountService)
        {
            _httpClient = httpClient;
            _discountService = discountService;
        }

        public async Task AddBasketItem(BasketItemViewModel basketItemViewModel)
        {
            BasketViewModel basket = await Get();

            if (basket is not null)
            {
                if (basket.BasketItemViewModels.Any(x => x.CourseId == basketItemViewModel.CourseId) is false)
                {
                    basket.BasketItemViewModels.Add(basketItemViewModel);

                    await SaveOrUpdate(basket);
                }
            }
            else
            {
                BasketViewModel basketViewModelNew = new();

                basketViewModelNew.BasketItemViewModels = new List<BasketItemViewModel> { basketItemViewModel };

                await SaveOrUpdate(basket);
            }
        }

        public async Task<bool?> ApplyDiscount(string code)
        {
            await CancelApplyDiscount();

            var basket = await Get();

            if (basket is null) return false;

            var discountViewModel = await _discountService.GetDiscountByCode(code);

            if(discountViewModel is null) return false;

            basket.DiscountCode = discountViewModel.Code;

           return await SaveOrUpdate(basket);
        }

        public async Task<bool?> CancelApplyDiscount()
        {
            var basket = await Get();

            if (basket is null) return false;

            basket.DiscountCode = default;

            return await SaveOrUpdate(basket);
        }

        public async Task<bool?> Delete()
        {
            var response = await _httpClient.DeleteAsync("basket/delete");

            return response != null && response.IsSuccessStatusCode;
        }

        public async Task<bool?> DeleteBasketItem(string courseId)
        {
            var basket = await Get();

            if (basket == null)
            {
                return false;
            }

            var basketItem = basket.BasketItemViewModels.First(x => x.CourseId == courseId);

            bool isDeleted = basket.BasketItemViewModels.Remove(basketItem);

            if (isDeleted is false) return false;

            basket.DiscountCode = default;

            return await SaveOrUpdate(basket);
        }

        public async Task<BasketViewModel> Get()
        {
            var response = await _httpClient.GetAsync("basket/get");

            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadFromJsonAsync<SuccessResponse<BasketViewModel>>();

                var basket = res.Data;

                if(string.IsNullOrEmpty(basket.DiscountCode) is false)
                {
                    DiscountViewModel discountViewModel = await _discountService
                                            .GetDiscountByCode(basket.DiscountCode);

                    basket.BasketItemViewModels.ForEach(x =>
                    {
                        x.DiscountAppliedPrice = x.Price - (x.Price * discountViewModel.Rate) / 100;
                    });
                }

                return basket;
            }

            return default;
        }

        public async Task<bool> SaveOrUpdate(BasketViewModel basketViewModel)
        {
            var response = await _httpClient.PostAsJsonAsync<BasketViewModel>("basket/saveorupdate", basketViewModel);

            return response.IsSuccessStatusCode;
        }
    }
}
