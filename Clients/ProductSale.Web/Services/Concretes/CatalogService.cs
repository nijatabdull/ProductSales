using ProductSale.Web.Models.Catalog;
using ProductSale.Web.Services.Abstractions;
using ProductSale.Shared.Infrastructure.Response;

namespace ProductSale.Web.Services.Concretes
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AddCourseAsync(CourseViewModel course)
        {
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

            SuccessResponse<List<CourseViewModel>> successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse<List<CourseViewModel>>>();

            return successResponse.Data;
        }

        public async Task<CourseViewModel> GetCourseByIdAsync(string courseId)
        {
            var response = await _httpClient.GetAsync("course/get-by-id?id=" + courseId);

            if (response.IsSuccessStatusCode is false)
                return default;

            SuccessResponse<CourseViewModel> successResponse = await response.Content.ReadFromJsonAsync<SuccessResponse<CourseViewModel>>();

            return successResponse.Data;
        }

        public Task<IEnumerable<CourseViewModel>> GetCoursesByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateCourseAsync(CourseViewModel course)
        {
            var response = await _httpClient.PutAsJsonAsync("Catalog/Update", course);

            return response.IsSuccessStatusCode;
        }
    }
}
