using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProductSale.Web.Models.Catalog
{
    public class CourseUpdateInput
    {
        public string Id { get; set; }

        [Display(Name = "Kurs adı")]
        public string Name { get; set; }

        [Display(Name = "Kurs açıqlama")]
        public string Description { get; set; }

        [Display(Name = "Kurs qiymət")]
        public decimal Price { get; set; }

        public string UserId { get; set; }

        public string Picture { get; set; }

        [JsonPropertyName("featureDto")]
        public FeatureViewModel Feature { get; set; }

        [Display(Name = "Kurs kateqoriya")]
        public string CategoryId { get; set; }

        [Display(Name = "Kurs foto")]
        public IFormFile? PhotoFormFile { get; set; }
    }
}
