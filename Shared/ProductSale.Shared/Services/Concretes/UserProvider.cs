using Microsoft.AspNetCore.Http;
using ProductSale.Shared.Services.Abstractions;

namespace ProductSale.Shared.Services.Concretes
{
    public class UserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId=> _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;
    }
}
