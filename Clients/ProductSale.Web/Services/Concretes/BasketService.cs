using ProductSale.Shared.Infrastructure.Response;
using ProductSale.Shared.Services.Abstractions;
using ProductSale.Web.Models.Basket;
using ProductSale.Web.Models.Discount;
using ProductSale.Web.Services.Abstractions;

namespace ProductSale.Web.Services.Concretes
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;
        private readonly IDiscountService _discountService;
        private readonly IUserProvider _userProvider;

        public BasketService(HttpClient httpClient, IDiscountService discountService, IUserProvider userProvider)
        {
            _httpClient = httpClient;
            _discountService = discountService;
            _userProvider = userProvider;
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
                BasketViewModel basketViewModelNew = new()
                {
                    UserId = _userProvider.GetUserId
                };

                basketViewModelNew.BasketItemViewModels = new List<BasketItemViewModel> { basketItemViewModel };

                await SaveOrUpdate(basketViewModelNew);
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
