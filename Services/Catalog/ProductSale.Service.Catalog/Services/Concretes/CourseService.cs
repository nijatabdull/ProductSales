using AutoMapper;
using Mass =MassTransit;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductSale.Service.Catalog.Dtos;
using ProductSale.Service.Catalog.Models;
using ProductSale.Service.Catalog.Services.Abstractions;
using ProductSale.Service.Catalog.Statics.Models;
using ProductSale.Shared.Infrastructure.Response;
using ProductSale.Shared.Infrastructure.Response.Base;
using ProductSale.Shared.Infrastructure.MassTransit.Events;

namespace ProductSale.Service.Catalog.Services.Concretes
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Course> _courses;
        private readonly IMongoCollection<Category> _categories;
        private readonly IMapper _mapper;
        private readonly Mass.IPublishEndpoint _publishEndpoint;

        public CourseService(IOptionsSnapshot<DatabaseConfig> optionsSnapshot,
                               IMapper mapper, Mass.IPublishEndpoint publishEndpoint)
        {
            MongoClient mongoClient = new(optionsSnapshot.Value.ConnectionString);
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(optionsSnapshot.Value.DatabaseName);
            _courses = mongoDatabase.GetCollection<Course>(optionsSnapshot.Value.CourseListName);
            _categories = mongoDatabase.GetCollection<Category>(optionsSnapshot.Value.CategoryListName);

            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<Response> CreateAsync(CourseDto courseDto)
        {
            if (courseDto is null)
                return new ErrorResponse("courseDto is null");

            Course course = _mapper.Map<Course>(courseDto);

            await _courses.InsertOneAsync(course);

            return new SuccessResponse<CourseDto>(courseDto);
        }

       

        public async Task<Response> GetAllAsync()
        {
            List<Course> courses = await _courses.Find(x => true).ToListAsync();

            if(courses is null)
                return new ErrorResponse("courses is null");

            if (courses.Any())
            {
                foreach (Course course in courses)
                {
                    course.Category = await _categories.Find(x => x.Id == course.CategoryId).FirstOrDefaultAsync();
                }
            }

            return new SuccessResponse<IEnumerable<CourseDto>>(_mapper.Map<IEnumerable<CourseDto>>(courses));
        }

        public async Task<Response> GetByIdAsync(string id)
        {
            Course course = await _courses.Find(x=>x.Id == id).FirstOrDefaultAsync();

            if (course is null)
                return new ErrorResponse("course is null");

            course.Category = await _categories.Find(x => x.Id == course.CategoryId).FirstOrDefaultAsync();

            return new SuccessResponse<CourseDto>(_mapper.Map<CourseDto>(course));
        }

        public async Task<Response> UpdateAsync(CourseDto courseDto)
        {
            if (courseDto is null)
                return new ErrorResponse("courseDto is null");

            Course course = _mapper.Map<Course>(courseDto);

            try
            {
                var result = await _courses.FindOneAndReplaceAsync(x => x.Id == courseDto.Id, course);

                if (result is null)
                    return new ErrorResponse("Course not found");
            }
            catch (Exception exp)
            {

                throw;
            }

            await _publishEndpoint.Publish<CourseNameChangedEvent>(new CourseNameChangedEvent()
            {
                CourseId = course.Id,
                CourseName = course.Name,
            });

            return new SuccessResponse<CourseDto>(courseDto);
        }

        public async Task<Response> DeleteAsync(string id)
        {
            var result = await _courses.DeleteOneAsync(x=>x.Id == id);

            if(result is null)
                return new ErrorResponse("id is null");

            if (result.DeletedCount > 0)
                return new SuccessResponse<object>();
            else
                return new ErrorResponse("not deleted");
        }
    }
}
