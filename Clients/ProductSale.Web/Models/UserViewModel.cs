using System.Text.Json.Serialization;

namespace ProductSale.Web.Models
{
    [Serializable]
    public class UserViewModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
