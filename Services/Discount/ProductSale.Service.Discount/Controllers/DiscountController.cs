using Microsoft.AspNetCore.Mvc;
using ProductSale.Service.Discount.Controllers.Base;
using ProductSale.Service.Discount.Services.Abstractions;
using ProductSale.Shared.Services.Abstractions;

namespace ProductSale.Service.Discount.Controllers
{
    public class DiscountController : AncestorController
    {
        private readonly IDiscountService _discountService;
        private IUserProvider _userProvider;

        public DiscountController(IDiscountService discountService, IUserProvider userProvider)
        {
            _discountService = discountService;
            _userProvider = userProvider;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            return await ResultAsync(_discountService.All());
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            return await ResultAsync(_discountService.GetById(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetByCode(string code)
        {
            return await ResultAsync(_discountService.GetByCode(code, _userProvider.GetUserId));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Models.Discount discount)
        {
            return await ResultAsync(_discountService.Create(discount));
        }

        [HttpPut]
        public async Task<IActionResult> Update(Models.Discount discount)
        {
            return await ResultAsync(_discountService.Update(discount));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            return await ResultAsync(_discountService.Delete(id));
        }
    }
}
