using ProductSale.Web.Models.Payment;
using ProductSale.Web.Services.Abstractions;

namespace ProductSale.Web.Services.Concretes
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ReceivePayment(PaymentInputModel paymentInputModel)
        {
            var res = await _httpClient.GetAsync("payment/index");

            return res.IsSuccessStatusCode;
        }
    }
}
