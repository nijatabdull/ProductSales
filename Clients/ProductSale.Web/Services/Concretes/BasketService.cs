using ProductSale.Shared.Infrastructure.Response;
using ProductSale.Shared.Services.Abstractions;
using ProductSale.Web.Models.Basket;
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
                if (basket.BasketItems.Any(x => x.CourseId == basketItemViewModel.CourseId) is false)
                {
                    basket.BasketItems.Add(basketItemViewModel);

                    await SaveOrUpdate(basket);
                }
            }
            else
            {
                BasketViewModel basketViewModelNew = new()
                {
                    UserId = _userProvider.GetUserId
                };

                basketViewModelNew.BasketItems = new List<BasketItemViewModel> { basketItemViewModel };

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

            basket.ApplyDiscount(discountViewModel.Code, discountViewModel.Rate);

            return await SaveOrUpdate(basket);
        }

        public async Task<bool?> CancelApplyDiscount()
        {
            var basket = await Get();

            if (basket is null || basket.DiscountCode is null) return false;

            basket.CancelDiscount(); 

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

            var basketItem = basket.BasketItems.First(x => x.CourseId == courseId);

            bool isDeleted = basket.BasketItems.Remove(basketItem);

            if (isDeleted is false) return false;

            if (!basket.BasketItems.Any())
            {
                basket.DiscountCode = null;
            }

            return await SaveOrUpdate(basket);
        }

        public async Task<BasketViewModel> Get()
        {
            var response = await _httpClient.GetAsync("basket/get");

            if (response.IsSuccessStatusCode)
            {
                SuccessResponse<BasketViewModel> successResponse = await response
                            .Content.ReadFromJsonAsync<SuccessResponse<BasketViewModel>>();

                return successResponse.Data;
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
