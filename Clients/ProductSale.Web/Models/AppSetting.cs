namespace ProductSale.Web.Models
{
    public class AppSetting
    {
        public string BaseUrl { get; set; }
        public string IdentityBaseUrl { get; set; }
        public string PhotoStockUrl { get; set; }
        public ServicePath ServicePath { get; set; }
    }

    public class ServicePath
    {
        public string Catalog { get; set; }
        public string Photo { get; set; }
    }
}
