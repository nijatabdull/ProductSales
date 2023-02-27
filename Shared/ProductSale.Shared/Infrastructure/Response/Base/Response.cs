using System.Net;
using System.Text.Json.Serialization;

namespace ProductSale.Shared.Infrastructure.Response.Base
{
    public class Response
    {
        [JsonPropertyName("status")]
        public HttpStatusCode StatusCode { get; set; }

        [JsonPropertyName("success")]
        public bool Success
        {
            get
            {
                return StatusCode >= HttpStatusCode.OK && StatusCode < HttpStatusCode.Ambiguous;
            }
            set
            {
                _ = value;
            }
        }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        public Response(HttpStatusCode httpStatusCode)
        {
            StatusCode = httpStatusCode;
            Message = httpStatusCode.ToString();
        }
    }
}
