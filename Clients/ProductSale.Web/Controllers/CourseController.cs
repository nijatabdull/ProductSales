using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductSale.Shared.Services.Abstractions;
using ProductSale.Web.Models.Catalog;
using ProductSale.Web.Services.Abstractions;

namespace ProductSale.Web.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly IUserProvider _userProvider;

        public CourseController(ICatalogService catalogService, IUserProvider userProvider)
        {
            _catalogService = catalogService;
            _userProvider = userProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _catalogService.GetCoursesByUserIdAsync(_userProvider.GetUserId));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _catalogService.GetAllCategoriesAsync();

            ViewBag.categoryList = new SelectList(categories, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateInput courseCreateInput)
        {
            var categories = await _catalogService.GetAllCategoriesAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");
            if (!ModelState.IsValid)
            {
                return View();
            }
            courseCreateInput.UserId = _userProvider.GetUserId;

            await _catalogService.AddCourseAsync(courseCreateInput);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await _catalogService.DeleteCourseAsync(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            CourseViewModel course = await _catalogService.GetCourseByIdAsync(id);
            var categories = await _catalogService.GetAllCategoriesAsync();

            if (course == null)
            {
                //mesaj göster
                RedirectToAction(nameof(Index));
            }
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", course.Id);
            CourseUpdateInput courseUpdateInput = new()
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Price = course.Price,
                Feature = course.FeatureViewModel,
                CategoryId = course.CategoryId,
                UserId = course.UserId,
                Picture = course.Picture
            };

            return View(courseUpdateInput);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CourseUpdateInput courseUpdateInput)
        {
            var categories = await _catalogService.GetAllCategoriesAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", courseUpdateInput.Id);
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _catalogService.UpdateCourseAsync(courseUpdateInput);

            return RedirectToAction(nameof(Index));
        }
    }
}
