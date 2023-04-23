using System.Text.Json.Serialization;

namespace ProductSale.Web.Models.Catalog
{
    public class FeatureViewModel
    {
        [JsonPropertyName("duration")]
        public int Duration { get; set; }
    }
}
