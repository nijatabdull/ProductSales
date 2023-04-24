using System.ComponentModel.DataAnnotations;

namespace ProductSale.Web.Models.Catalog
{
    public class CourseCreateInput
    {
        [Display(Name = "Kurs adı")]
        public string Name { get; set; }

        [Display(Name = "Kurs açıqlama")]
        public string Description { get; set; }

        [Display(Name = "Kurs qiymət")]
        public decimal Price { get; set; }

        public string? Picture { get; set; }

        public string? UserId { get; set; }

        public FeatureViewModel Feature { get; set; }

        [Display(Name = "Kurs kategoriya")]
        public string CategoryId { get; set; }

        [Display(Name = "Kurs şəkil")]
        public IFormFile PhotoFormFile { get; set; }
    }
}
