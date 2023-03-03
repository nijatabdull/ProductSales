using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ProductSale.Shared.Infrastructure.Response;
using ProductSale.Web.Exceptions;
using ProductSale.Web.Services.Abstractions;

namespace ProductSale.Web.Handlers
{
    public class ResourceOwnerPasswordTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IIdentityService _identityService;
        private readonly ILogger<ResourceOwnerPasswordTokenHandler> _logger;

        public ResourceOwnerPasswordTokenHandler(IHttpContextAccessor contextAccessor, IIdentityService identityService, ILogger<ResourceOwnerPasswordTokenHandler> logger)
        {
            _contextAccessor = contextAccessor;
            _identityService = identityService;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string accessToken =await _contextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage httpResponseMessage = await base.SendAsync(request, cancellationToken);

            if(httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                SuccessResponse<TokenResponse> successResponse = await _identityService.GetAccessTokenByRefreshToken() as SuccessResponse<TokenResponse>;

                if(successResponse != null && successResponse.Data is not null)
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", successResponse.Data.AccessToken);

                    httpResponseMessage = await base.SendAsync(request, cancellationToken);
                }
            }

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException();
            }

            return httpResponseMessage;
        }
    }
}
