using ProductSale.Web.Models.Payment;

namespace ProductSale.Web.Services.Abstractions
{
    public interface IPaymentService
    {
        Task<bool> ReceivePayment(PaymentInputModel paymentInputModel);
    }
}
