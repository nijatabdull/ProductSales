using System.Text.Json.Serialization;

namespace ProductSale.Web.Models.Catalog
{
    public class CategoryViewModel
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
