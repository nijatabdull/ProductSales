using ProductSale.Web.Models.Photo;

namespace ProductSale.Web.Services.Abstractions
{
    public interface IPhotoService
    {
        Task<PhotoViewModel> Upload(IFormFile formFile);
        Task<bool> Delete(string url);
    }
}
