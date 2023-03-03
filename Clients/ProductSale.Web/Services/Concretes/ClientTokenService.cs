using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using ProductSale.Shared.Infrastructure.Response;
using ProductSale.Web.Models;
using ProductSale.Web.Services.Abstractions;
using System.Net.Http;

namespace ProductSale.Web.Services.Concretes
{
    public class ClientTokenService : IClientTokenService
    {
        private readonly AppSetting _appSetting;
        private readonly ClientSetting _clientSetting;
        private readonly IClientAccessTokenCache _clientAccessTokenCache;
        private readonly HttpClient _httpClient;

        public ClientTokenService(IOptions<AppSetting> appSetting,
            IOptions<ClientSetting> clientSetting,
            IClientAccessTokenCache clientAccessTokenCache, HttpClient httpClient)
        {
            _appSetting = appSetting.Value;
            _clientSetting = clientSetting.Value;
            _clientAccessTokenCache = clientAccessTokenCache;
            _httpClient = httpClient;
        }

        public async Task<string> GetTokenAsync()
        {
            var currentToken = await _clientAccessTokenCache.GetAsync("WebClientToken", default);

            if (currentToken is not null)
            {
                return currentToken.AccessToken;
            }

            DiscoveryDocumentResponse discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = _appSetting.IdentityBaseUrl,
                Policy = new DiscoveryPolicy()
                {
                    RequireHttps = false,
                }
            });

            if (discoveryDocument.IsError)
                return discoveryDocument.Error;

            ClientCredentialsTokenRequest clientCredentialsTokenRequest = new()
            {
                ClientId = _clientSetting.WebClient.ClientId,
                ClientSecret = _clientSetting.WebClient.ClientSecret,
                Address = discoveryDocument.TokenEndpoint
            };

            var newToken = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest);

            if (newToken.IsError)
                return newToken.Error;

            await _clientAccessTokenCache.SetAsync("WebClientToken", newToken.AccessToken, newToken.ExpiresIn, default);

            return newToken.AccessToken;
        }
    }
}
