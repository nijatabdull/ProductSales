using Microsoft.AspNetCore.Mvc;
using ProductSale.Service.Basket.Controllers.Base;
using ProductSale.Service.Basket.Dtos;
using ProductSale.Service.Basket.Services.Abstractions;
using ProductSale.Shared.Services.Abstractions;

namespace ProductSale.Service.Basket.Controllers
{
    public class BasketController : AncestorController
    {
        private readonly IBasketService _basketService;
        private readonly IUserProvider _userProvider;

        public BasketController(IBasketService basketService, IUserProvider userProvider)
        {
            _basketService = basketService;
            _userProvider = userProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await ResultAsync(_basketService.Get(_userProvider.GetUserId));
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdate(BasketDto basketDto)
        {
            return await ResultAsync(_basketService.SaveOrUpdate(basketDto));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            return await ResultAsync(_basketService.Delete(_userProvider.GetUserId));
        }
    }
}
