using ProductSale.Web.Models.Order;

namespace ProductSale.Web.Services.Abstractions
{
    public interface IOrderService
    {
        //sycn
        Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInputModel);

        //async - RABBIT MQ
        Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoInput checkoutInputModel);

        Task<List<OrderViewModel>> GetOrders();
    }
}
