using System.Text.Json.Serialization;

namespace ProductSale.Web.Models.Order
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string BuyerId { get; set; }
        [JsonPropertyName("orderItemDtos")]
        public List<OrderItemViewModel> OrderItemViewModels { get; set; }
    }
}
