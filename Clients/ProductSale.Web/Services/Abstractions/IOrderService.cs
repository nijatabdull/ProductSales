using ProductSale.Web.Models.Order;

namespace ProductSale.Web.Services.Abstractions
{
    public interface IOrderService
    {
        //sycn
        Task<OrderCreatedViewModel> CreateOrder(CheckoutInputModel checkoutInputModel);

        //async - RABBIT MQ
        Task SuspendOrder(CheckoutInputModel checkoutInputModel);

        Task<List<OrderViewModel>> GetOrders();
    }
}
