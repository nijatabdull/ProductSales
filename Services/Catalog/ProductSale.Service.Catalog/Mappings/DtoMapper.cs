using AutoMapper;
using ProductSale.Service.Catalog.Dtos;
using ProductSale.Service.Catalog.Models;

namespace ProductSale.Service.Catalog.Mappings
{
    public class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<CourseDto, Course>()
                .ForMember(x=>x.Category, y=>y.MapFrom(x=>x.CategoryDto))
                .ForMember(x=>x.Feature, y=>y.MapFrom(x=>x.FeatureDto))
                .ReverseMap();

            CreateMap<CategoryDto,Category>().ReverseMap();
            CreateMap<FeatureDto,Feature>().ReverseMap();
        }
    }
}
