using ProductSale.Service.Catalog.Dtos;
using ProductSale.Shared.Infrastructure.Response.Base;

namespace ProductSale.Service.Catalog.Services.Abstractions
{
    public interface ICourseService
    {
        Task<Response> GetAllAsync();
        Task<Response> CreateAsync(CourseDto courseDto);
        Task<Response> UpdateAsync(CourseDto courseDto);
        Task<Response> GetByIdAsync(string id);
        Task<Response> DeleteAsync(string id);
    }
}
