using Microsoft.AspNetCore.Mvc;
using ProductSale.Shared.Services.Abstractions;
using ProductSale.Web.Services.Abstractions;

namespace ProductSale.Web.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly IUserProvider _userProvider;
        private readonly IPhotoService _photoService;

        public CourseController(ICatalogService catalogService, IUserProvider userProvider, IPhotoService photoService)
        {
            _catalogService = catalogService;
            _userProvider = userProvider;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _catalogService.GetAllCoursesAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _catalogService.GetAllCategoriesAsync();

            using Stream stream = System.IO.File.OpenRead("C:\\Users\\Admin\\Pictures\\test.jfif");

            FormFile formFile = new (stream, 0, stream.Length, "photo", "test.jfif");

            var photo = await _photoService.Upload(formFile);

            await _catalogService.AddCourseAsync(new Models.Catalog.CourseViewModel()
            {
                CategoryId = categories.First().Id,
                Name = "Java",
                Price = 31,
                Picture = photo?.Url,
                UserId = _userProvider.GetUserId,
            });

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await _catalogService.DeleteCourseAsync(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update()
        {
            await _catalogService.UpdateCourseAsync(new Models.Catalog.CourseViewModel()
            {
                Id = "6404f7124f728397ad79567c",
                Name = "Python",
                CategoryId = "63e47fe7ee26be4f885d321f",
                CategoryViewModel = new Models.Catalog.CategoryViewModel(),
                FeatureViewModel= new Models.Catalog.FeatureViewModel(),
                Picture = "fuck",
                Price= 31,
                UserId = _userProvider.GetUserId
            });

            return RedirectToAction("Index");
        }
    }
}
