using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
           var course = await _catalogService.GetCourseByIdAsync(courseId);

            await _basketService.AddBasketItem(new Models.Basket.BasketItemViewModel()
            {
                CourseId= courseId,
                CourseName= course.Name,
                Price = 31,
                Quantity = 1
            });

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string courseId)
        {
            await _basketService.DeleteBasketItem(courseId);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ApplyDiscount()
        {
            string discountCode = "ABC";

            await _basketService.ApplyDiscount(discountCode);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> CancelApplyDiscount()
        {
            await _basketService.CancelApplyDiscount();

            return RedirectToAction("Index");
        }
    }
}
