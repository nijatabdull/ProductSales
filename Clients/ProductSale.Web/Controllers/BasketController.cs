using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductSale.Web.Models.Catalog;
using ProductSale.Web.Models.Discount;
using ProductSale.Web.Services.Abstractions;

namespace ProductSale.Web.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;

        public BasketController(ICatalogService catalogService, IBasketService basketService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _basketService.Get());
        }

        [HttpGet]
        public async Task<IActionResult> AddBasketItem(string courseId)
        {
           CourseViewModel course = await _catalogService.GetCourseByIdAsync(courseId);

            await _basketService.AddBasketItem(new Models.Basket.BasketItemViewModel()
            {
                CourseId= courseId,
                CourseName= course.Name,
                Price = course.Price,
            });

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string courseId)
        {
            await _basketService.DeleteBasketItem(courseId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ApplyDiscount(DiscountApplyCode discountApplyCode)
        {
            if (!ModelState.IsValid)
            {
                TempData["discountError"] = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).First();
                return RedirectToAction(nameof(Index));
            }
            var discountStatus = await _basketService.ApplyDiscount(discountApplyCode.Code);

            TempData["discountStatus"] = discountStatus;
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> CancelApplyDiscount()
        {
            await _basketService.CancelApplyDiscount();

            return RedirectToAction("Index");
        }
    }
}
