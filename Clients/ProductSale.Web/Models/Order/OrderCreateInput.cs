using System.Text.Json.Serialization;

namespace ProductSale.Web.Models.Order
{
    public class OrderCreateInput
    {
        public OrderCreateInput()
        {
            OrderItemViewModels = new List<OrderItemViewModel>();
        }

        [JsonPropertyName("addressDto")]
        public AddressCreateModel AddressCreateModel { get; set; }
        public string BuyerId { get; set; }
        [JsonPropertyName("orderItemDtos")]
        public List<OrderItemViewModel> OrderItemViewModels { get; set; }
    }
}
