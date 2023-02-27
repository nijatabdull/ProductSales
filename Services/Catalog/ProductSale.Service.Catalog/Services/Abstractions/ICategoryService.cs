using ProductSale.Service.Catalog.Dtos;
using ProductSale.Shared.Infrastructure.Response.Base;

namespace ProductSale.Service.Catalog.Services.Abstractions
{
    public interface ICategoryService
    {
        Task<Response> GetAllAsync();
        Task<Response> CreateAsync(CategoryDto categoryDto);
        Task<Response> GetByIdAsync(string id);
    }
}
