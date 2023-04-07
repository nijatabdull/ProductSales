using ProductSale.Web.Models.Order;
using System.Text.Json.Serialization;

namespace ProductSale.Web.Models.Payment
{
    public class PaymentInputModel
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public decimal TotalPrice { get; set; }
        [JsonPropertyName("order")]
        public OrderCreateInput OrderCreateInput { get; set; }
    }
}
