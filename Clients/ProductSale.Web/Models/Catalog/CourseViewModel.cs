using System.Text.Json.Serialization;

namespace ProductSale.Web.Models.Catalog
{
    public class CourseViewModel
    {
        public string? Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [JsonPropertyName("picture")]
        public string Picture { get; set; }
        [JsonPropertyName("categoryId")]
        public string CategoryId { get; set; }

        public CategoryViewModel CategoryViewModel { get; set; }
        public FeatureViewModel FeatureViewModel { get; set; }
    }
}
