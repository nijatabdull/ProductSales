using Microsoft.AspNetCore.Mvc;
using ProductSale.Service.Catalog.Controllers.Base;
using ProductSale.Service.Catalog.Dtos;
using ProductSale.Service.Catalog.Services.Abstractions;

namespace ProductSale.Service.Catalog.Controllers
{
    public class CategoryController : AncestorController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService) 
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
           return await ResultAsync(_categoryService.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            return await ResultAsync(_categoryService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto CategoryDto)
        {
            return await ResultAsync(_categoryService.CreateAsync(CategoryDto));
        }
    }
}
