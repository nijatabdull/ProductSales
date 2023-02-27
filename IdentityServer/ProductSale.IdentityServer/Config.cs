using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace ProductSale.IdentityServer;

public static class Config
{
    public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
    {
        new ApiResource("catalog_resource"){Scopes = { "catolog_fullpermission" } },
        new ApiResource("photostock_resource"){Scopes = { "photostock_fullpermission" } },
        new ApiResource("basket_resource"){Scopes = { "basket_fullpermission" } },
        new ApiResource("discount_resource"){Scopes = { "discount_fullpermission" } },
        new ApiResource("order_resource"){Scopes = { "order_fullpermission" } },
        new ApiResource("payment_resource"){Scopes = { "payment_fullpermission" } },
        new ApiResource("gateway_resource"){Scopes = { "gateway_fullpermission" } },
        new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
    };

    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource()
            {
                Name = "roles",
                DisplayName = "Roles",
                Description = "User roles",
                UserClaims = new []{"role"}
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("catolog_fullpermission"),
            new ApiScope("photostock_fullpermission"),
            new ApiScope("basket_fullpermission"),
            new ApiScope("discount_fullpermission"),
            new ApiScope("order_fullpermission"),
            new ApiScope("payment_fullpermission"),
            new ApiScope("gateway_fullpermission"),
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "WebMvcCLient",
                ClientName = "Asp.Net Core MVC",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("mvc-12345".Sha256()) },

                AllowedScopes = { "gateway_fullpermission", "catolog_fullpermission", "photostock_fullpermission", IdentityServerConstants.LocalApi.ScopeName }
            },

             new Client
            {
                ClientId = "WebMvcCLientForUser",
                ClientName = "Asp.Net Core MVC For User",

                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets = { new Secret("mvc-12345".Sha256()) },

                AllowedScopes = {"basket_fullpermission","discount_fullpermission",
                     "order_fullpermission", "payment_fullpermission","gateway_fullpermission",
                                IdentityServerConstants.StandardScopes.OpenId,
                                IdentityServerConstants.StandardScopes.Email,
                                IdentityServerConstants.StandardScopes.Profile,
                                IdentityServerConstants.StandardScopes.OfflineAccess,
                                IdentityServerConstants.LocalApi.ScopeName,
                                "roles"},

                AccessTokenLifetime = 3600,
                RefreshTokenExpiration = TokenExpiration.Absolute,
                AbsoluteRefreshTokenLifetime= 60 * 60 * 24 * 60,
                RefreshTokenUsage = TokenUsage.ReUse,
                AllowOfflineAccess= true,
            },
        };
}
