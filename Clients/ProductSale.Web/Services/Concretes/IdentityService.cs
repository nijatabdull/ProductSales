using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ProductSale.Shared.Infrastructure.Response;
using ProductSale.Shared.Infrastructure.Response.Base;
using ProductSale.Web.Models;
using ProductSale.Web.Services.Abstractions;
using System.Globalization;
using System.Security.Claims;

namespace ProductSale.Web.Services.Concretes
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ClientSetting _clientSetting;
        private readonly AppSetting _appSetting;

        public IdentityService(HttpClient httpClient, IHttpContextAccessor contextAccessor, IOptions<ClientSetting> clientSetting, IOptions<AppSetting> appSetting)
        {
            _httpClient = httpClient;
            _contextAccessor = contextAccessor;
            _clientSetting = clientSetting.Value;
            _appSetting = appSetting.Value;
        }

        public async Task<Response> GetAccessTokenByRefreshToken()
        {
            DiscoveryDocumentResponse discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = _appSetting.IdentityBaseUrl,
                Policy = new DiscoveryPolicy()
                {
                    RequireHttps = false,
                }
            });

            if (discoveryDocument.IsError)
                return new ErrorResponse(discoveryDocument.Error);

            string refreshToken = await _contextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            RefreshTokenRequest refreshTokenRequest = new()
            {
                RefreshToken = refreshToken,
                ClientId = _clientSetting.WebClientForUser.ClientId,
                ClientSecret = _clientSetting.WebClientForUser.ClientSecret,
                Address = discoveryDocument.TokenEndpoint
            };

            TokenResponse tokenResponse = await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if (tokenResponse.IsError)
                return new ErrorResponse(tokenResponse.Error);

            var auths = new List<AuthenticationToken>()
            {
                new AuthenticationToken()
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = tokenResponse.AccessToken
                },
                new AuthenticationToken()
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = tokenResponse.RefreshToken
                },
                new AuthenticationToken()
                {
                    Name = OpenIdConnectParameterNames.ExpiresIn,
                    Value = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)
                },
            };

            AuthenticateResult authenticateResult = await _contextAccessor.HttpContext.AuthenticateAsync();

            AuthenticationProperties authenticationProperties = authenticateResult.Properties;

            authenticationProperties.StoreTokens(auths);

            _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                                            authenticateResult.Principal,
                                                                            authenticationProperties);
            return new SuccessResponse<TokenResponse>(tokenResponse);
        }

        public async Task<Response> RevokeRefreshToken()
        {
            DiscoveryDocumentResponse discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = _appSetting.IdentityBaseUrl,
                Policy = new DiscoveryPolicy()
                {
                    RequireHttps = false,
                }
            });

            if (discoveryDocument.IsError)
                return new ErrorResponse(discoveryDocument.Error);

            string refreshToken = await _contextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            TokenRevocationRequest tokenRevocationRequest = new()
            {
                ClientId = _clientSetting.WebClientForUser.ClientId,
                ClientSecret = _clientSetting.WebClientForUser.ClientSecret,
                Address = discoveryDocument.TokenEndpoint,
                Token = refreshToken,
                TokenTypeHint = "refresh_token"
            };

            await _httpClient.RevokeTokenAsync(tokenRevocationRequest);

            return new SuccessResponse<bool>(true);
        }

        public async Task<Response> SignIn(SignInInput signInInput)
        {
            DiscoveryDocumentResponse discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = _appSetting.IdentityBaseUrl,
                Policy = new DiscoveryPolicy()
                {
                    RequireHttps = false,
                }
            });

            if (discoveryDocument.IsError)
                return new ErrorResponse(discoveryDocument.Error);

            PasswordTokenRequest passwordTokenRequest = new()
            {
                ClientId = _clientSetting.WebClientForUser.ClientId,
                ClientSecret = _clientSetting.WebClientForUser.ClientSecret,
                UserName = signInInput.Email,
                Password = signInInput.Password,
                Address = discoveryDocument.TokenEndpoint
            };

            TokenResponse tokenResponse = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

            if (tokenResponse.IsError)
                return new ErrorResponse(tokenResponse.Error);

            UserInfoRequest userInfoRequest = new()
            {
                Token = tokenResponse.AccessToken,
                Address = discoveryDocument.UserInfoEndpoint
            };

            UserInfoResponse userInfoResponse = await _httpClient.GetUserInfoAsync(userInfoRequest);

            if (userInfoResponse.IsError)
                return new ErrorResponse("userInfoResponse was not found");

            ClaimsIdentity claimsIdentity = new(userInfoResponse.Claims,
                                                CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");

            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

            AuthenticationProperties authenticationProperties = new();

            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken()
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = tokenResponse.AccessToken
                },
                new AuthenticationToken()
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = tokenResponse.RefreshToken
                },
                new AuthenticationToken()
                {
                    Name = OpenIdConnectParameterNames.ExpiresIn,
                    Value = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)
                },
            });

            authenticationProperties.IsPersistent = signInInput.IsRemember;

            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                                                    claimsPrincipal, authenticationProperties);

            return new SuccessResponse<bool>(true);
        }
    }
}
