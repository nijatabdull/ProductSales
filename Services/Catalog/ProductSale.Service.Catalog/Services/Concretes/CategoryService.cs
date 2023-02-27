using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductSale.Service.Catalog.Dtos;
using ProductSale.Service.Catalog.Models;
using ProductSale.Service.Catalog.Services.Abstractions;
using ProductSale.Service.Catalog.Statics.Models;
using ProductSale.Shared.Infrastructure.Response;
using ProductSale.Shared.Infrastructure.Response.Base;

namespace ProductSale.Service.Catalog.Services.Concretes
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categories;
        private readonly IMapper _mapper;
        public CategoryService(IOptionsSnapshot<DatabaseConfig> optionsSnapshot,
                               IMapper mapper)
        {
            MongoClient mongoClient = new(optionsSnapshot.Value.ConnectionString);
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(optionsSnapshot.Value.DatabaseName);
            _categories = mongoDatabase.GetCollection<Category>(optionsSnapshot.Value.CategoryListName);

            _mapper = mapper;
        }

        public async Task<Response> GetAllAsync()
        {
            IEnumerable<Category> categories = await _categories.Find(x => true).ToListAsync();

            return new SuccessResponse<IEnumerable<CategoryDto>>(_mapper.Map<IEnumerable<CategoryDto>>(categories));
        }

        public async Task<Response> CreateAsync(CategoryDto categoryDto)
        {
            if(categoryDto is null)
                return new ErrorResponse("categoryDto is null");

            Category category = _mapper.Map<Category>(categoryDto);

            await _categories.InsertOneAsync(category);

            return new SuccessResponse<CategoryDto>(categoryDto);
        }

        public async Task<Response> GetByIdAsync(string id)
        {
            Category category = await _categories.Find(x=>x.Id== id).FirstOrDefaultAsync();

            if(category is null)
                return new ErrorResponse("categoryDto is null");

            return new SuccessResponse<CategoryDto>(_mapper.Map<CategoryDto>(category));
        }
    }
}
