using System.Net;
using System.Text.Json.Serialization;

namespace ProductSale.Shared.Infrastructure.Response
{
    public class ErrorResponse : Base.Response
    {
        [JsonPropertyName("errors")]
        public IEnumerable<string> Errors { get; set; }

        public ErrorResponse(HttpStatusCode httpStatusCode, string error) :
            base(httpStatusCode)
        {
            Errors = new List<string>
            {
                error
            };
        }

        public ErrorResponse(string error) :
            base(HttpStatusCode.BadRequest)
        {
            Errors = new List<string>
            {
                error
            };
        }

        public ErrorResponse(HttpStatusCode httpStatusCode, IEnumerable<string> errors) :
            base(httpStatusCode)
        {
            Errors = errors?.ToList();
        }
    }
}
