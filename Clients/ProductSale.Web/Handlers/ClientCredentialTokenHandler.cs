using ProductSale.Web.Exceptions;
using ProductSale.Web.Services.Abstractions;
using System.Net.Http.Headers;

namespace ProductSale.Web.Handlers
{
    public class ClientCredentialTokenHandler : DelegatingHandler
    {
        private readonly IClientTokenService _clientTokenService;

        public ClientCredentialTokenHandler(IClientTokenService clientTokenService)
        {
            _clientTokenService = clientTokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string token = await _clientTokenService.GetTokenAsync();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await base.SendAsync(request, cancellationToken);

            if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException();
            }

            return response;
        }
    }
}
