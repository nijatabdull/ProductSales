using System.Net;
using System.Text.Json.Serialization;

namespace ProductSale.Shared.Infrastructure.Response
{
    public class SuccessResponse<T> : Base.Response
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }

        public SuccessResponse(T data, HttpStatusCode httpStatusCode) :
            base(httpStatusCode)
        {
            Data = data;
        }

        public SuccessResponse(T data) :
            base(HttpStatusCode.OK)
        {
            Data = data;
        }

        public SuccessResponse() :
             base(HttpStatusCode.OK)
        {
        }
    }
}
