using Microsoft.AspNetCore.Mvc;

namespace ProductSale.Service.Photo.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PhotoController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Save(IFormFile formFile, CancellationToken cancellationToken)
        {
            if (formFile is null || formFile.Length == 0)
                return BadRequest();

            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos", formFile.FileName);

            using Stream stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);

            await formFile.CopyToAsync(stream, cancellationToken);

            return Ok(formFile.FileName);
        }

        [HttpDelete]
        public IActionResult Delete(string photoUrl)
        {
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos", photoUrl);

            if (System.IO.File.Exists(fullPath) is false)
                return NotFound();

            System.IO.File.Delete(fullPath);

            return Ok();
        }
    }
}
