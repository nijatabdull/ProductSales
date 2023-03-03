using ProductSale.Shared.Infrastructure.Response.Base;
using ProductSale.Web.Models;

namespace ProductSale.Web.Services.Abstractions
{
    public interface IUserService
    {
        Task<UserViewModel> GetUser();
    }
}
