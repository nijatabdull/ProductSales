using Microsoft.AspNetCore.Mvc;
using ProductSale.Service.Catalog.Controllers.Base;
using ProductSale.Service.Catalog.Dtos;
using ProductSale.Service.Catalog.Services.Abstractions;

namespace ProductSale.Service.Catalog.Controllers
{
    public class CourseController : AncestorController
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService) 
        {
            _courseService= courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
           return await ResultAsync(_courseService.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            return await ResultAsync(_courseService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseDto courseDto)
        {
            return await ResultAsync(_courseService.CreateAsync(courseDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(CourseDto courseDto)
        {
            return await ResultAsync(_courseService.UpdateAsync(courseDto));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            return await ResultAsync(_courseService.DeleteAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            return await ResultAsync(_courseService.GetAllByUserIdAsync(userId));
        }
    }
}
