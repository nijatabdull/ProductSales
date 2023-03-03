using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ProductSale.Shared.Infrastructure.Response;
using ProductSale.Shared.Infrastructure.Response.Base;
using ProductSale.Web.Models;
using ProductSale.Web.Services.Abstractions;

namespace ProductSale.Web.Services.Concretes
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserViewModel> GetUser()
        {
            UserViewModel userViewModel = await _httpClient.GetFromJsonAsync<UserViewModel>("api/user/getuser");

            return userViewModel;
        }
    }
}
