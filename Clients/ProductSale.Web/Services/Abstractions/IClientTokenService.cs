namespace ProductSale.Web.Services.Abstractions
{
    public interface IClientTokenService
    {
        Task<string> GetTokenAsync();
    }
}
