using ProductSale.Web.Models.Catalog;

namespace ProductSale.Web.Services.Abstractions
{
    public interface ICatalogService
    {
        Task<IEnumerable<CourseViewModel>> GetAllCoursesAsync();
        Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();
        Task<IEnumerable<CourseViewModel>> GetCoursesByUserIdAsync(string userId);
        Task<bool> DeleteCourseAsync(string courseId);  
        Task<CourseViewModel> GetCourseByIdAsync(string courseId);
        Task<bool> AddCourseAsync(CourseViewModel course);
        Task<bool> UpdateCourseAsync(CourseViewModel course);
    }
}
