﻿namespace ProductSale.Service.Catalog.Dtos
{
    public class CourseDto
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }

        public CategoryDto? CategoryDto { get; set; }
        public FeatureDto? FeatureDto { get; set; }
    }
}
