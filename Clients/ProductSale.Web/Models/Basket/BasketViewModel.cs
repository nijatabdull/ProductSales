using System.Text.Json.Serialization;

namespace ProductSale.Web.Models.Basket
{
    public class BasketViewModel
    {
        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        [JsonPropertyName("basketItemDtos")]
        public List<BasketItemViewModel> BasketItemViewModels { get; set; }
        public decimal TotalPrice
        {
            get => BasketItemViewModels
                .Sum(x => x.DiscountAppliedPrice.HasValue ? x.DiscountAppliedPrice.Value : x.Price * x.Quantity);
        }
    }
}
