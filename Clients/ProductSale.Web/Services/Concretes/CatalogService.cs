using ProductSale.Web.Models.Catalog;
using ProductSale.Web.Services.Abstractions;
using ProductSale.Shared.Infrastructure.Response;
using ProductSale.Web.Models;
using ProductSale.Web.Models.Photo;

namespace ProductSale.Web.Services.Concretes
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly IPhotoService _photoService;
        private readonly string _photoStockUrl;

        public CatalogService(HttpClient httpClient, IPhotoService photoService, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _photoService = photoService;
            _photoStockUrl = configuration["AppSetting:PhotoStockUrl"];
        }

        private string GetPhotoStockUrl(string photoUrl)
        {
            return $"{_photoStockUrl}/photos/{photoUrl}";
        }

        public async Task<bool> AddCourseAsync(CourseCreateInput course)
        {
            var resultPhotoService = await _photoService.Upload(course.PhotoFormFile);

            if (resultPhotoService != null)
            {
                course.Picture = resultPhotoService.Url;
            }

            var response = await _httpClient.PostAsJsonAsync("course/Create", course);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCourseAsync(string courseId)
        {
            var response = await _httpClient.DeleteAsync("course/Delete?id=" + courseId);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("category/all");

            if (response.IsSuccessStatusCode is false)
                return default;

            SuccessResponse<List<CategoryViewModel>> successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse<List<CategoryViewModel>>>();

            return successResponse.Data;
        }

        public async Task<IEnumerable<CourseViewModel>> GetAllCoursesAsync()
        {
            var response = await _httpClient.GetAsync("course/all");

            if (response.IsSuccessStatusCode is false)
                return default;

            SuccessResponse<List<CourseViewModel>> successResponse = await response
                        .Content.ReadFromJsonAsync<SuccessResponse<List<CourseViewModel>>>();

            successResponse.Data.ForEach(x =>
            {
                x.Picture = GetPhotoStockUrl(x.Picture);
            });

            return successResponse.Data;
        }

        public async Task<CourseViewModel> GetCourseByIdAsync(string courseId)
        {
            var response = await _httpClient.GetAsync("course/get-by-id?id=" + courseId);

            if (response.IsSuccessStatusCode is false)
                return default;

            SuccessResponse<CourseViewModel> successResponse = await response
                    .Content.ReadFromJsonAsync<SuccessResponse<CourseViewModel>>();

            successResponse.Data.Picture = GetPhotoStockUrl(successResponse.Data.Picture);

            return successResponse.Data;
        }

        public async Task<IEnumerable<CourseViewModel>> GetCoursesByUserIdAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"course/get-all-by-user-id?userId={userId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<SuccessResponse<List<CourseViewModel>>>();

            responseSuccess.Data.ForEach(x =>
            {
                x.Picture = GetPhotoStockUrl(x.Picture);
            });

            return responseSuccess.Data;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateInput course)
        {
            if(course.PhotoFormFile is not null)
            {
                PhotoViewModel resultPhotoService = await _photoService.Upload(course.PhotoFormFile);

                if (resultPhotoService != null)
                {
                    await _photoService.Delete(course.Picture);
                    course.Picture = resultPhotoService.Url;
                }
            }
            else
            {
                course.Picture = course.Picture[(course.Picture.LastIndexOf("/") + 1)..];
            }

            var response = await _httpClient.PutAsJsonAsync("course/update", course);

            return response.IsSuccessStatusCode;
        }
    }
}
