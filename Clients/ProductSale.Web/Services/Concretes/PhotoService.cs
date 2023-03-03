using ProductSale.Web.Models.Photo;
using ProductSale.Web.Services.Abstractions;

namespace ProductSale.Web.Services.Concretes
{
    public class PhotoService : IPhotoService
    {
        private readonly HttpClient _httpClient;

        public PhotoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> Delete(string url)
        {
            var response = await _httpClient.DeleteAsync("photo/delete?photoUrl=" + url);

            return response!= null && response.IsSuccessStatusCode;
        }

        public async Task<PhotoViewModel> Upload(IFormFile formFile)
        {
            if (formFile is null || formFile.Length < 1)
                return null;

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(formFile.FileName)}";

            using MemoryStream memoryStream = new();

            formFile.CopyTo(memoryStream);

            MultipartFormDataContent formData = new()
            {
                { new ByteArrayContent(memoryStream.ToArray()), "formFile", fileName }
            };

            var response = await _httpClient.PostAsync("photo/save", formData);

            if (response.IsSuccessStatusCode)
            {
               return new PhotoViewModel()
               {
                   Url = await response.Content.ReadFromJsonAsync<string>()
               };
            }

            return default;
        }
    }
}
