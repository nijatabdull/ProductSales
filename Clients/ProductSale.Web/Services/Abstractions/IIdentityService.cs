using IdentityModel.Client;
using ProductSale.Shared.Infrastructure.Response.Base;
using ProductSale.Web.Models;

namespace ProductSale.Web.Services.Abstractions
{
    public interface IIdentityService
    {
        Task<Response> SignIn(SignInInput signInInput);

        Task<Response> GetAccessTokenByRefreshToken();

        Task<Response> RevokeRefreshToken();
    }
}
