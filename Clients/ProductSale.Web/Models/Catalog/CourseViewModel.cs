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
        [JsonPropertyName("description")]
        public string Description { get; set; }

        public string ShortDescription
        {
            get
            {
                if (Description is not null)
                {
                    return Description.Length > 100 ? Description.Substring(0, 100) + "..." : Description;
                }
                return string.Empty;
            }
        }

        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("categoryDto")]
        public CategoryViewModel CategoryViewModel { get; set; }

        [JsonPropertyName("featureDto")]
        public FeatureViewModel FeatureViewModel { get; set; }
    }
}
