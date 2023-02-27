using Microsoft.AspNetCore.Mvc;

namespace ProductSale.Service.FakePayment.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PaymentController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
