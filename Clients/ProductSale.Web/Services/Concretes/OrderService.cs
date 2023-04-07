using ProductSale.Shared.Infrastructure.Response;
using ProductSale.Shared.Services.Abstractions;
using ProductSale.Web.Models.Order;
using ProductSale.Web.Models.Payment;
using ProductSale.Web.Services.Abstractions;

namespace ProductSale.Web.Services.Concretes
{
    public class OrderService : IOrderService
    {
        private readonly IPaymentService _paymentService;
        private readonly HttpClient _httpClient;
        private readonly IBasketService _basketService;
        private readonly IUserProvider _userProvider;

        public OrderService(IPaymentService paymentService,
            HttpClient httpClient, IBasketService basketService, IUserProvider userProvider)
        {
            _paymentService = paymentService;
            _httpClient = httpClient;
            _basketService = basketService;
            _userProvider = userProvider;
        }

        public async Task<OrderCreatedViewModel> CreateOrder(CheckoutInputModel checkoutInputModel)
        {
            var basket = await _basketService.Get();

            PaymentInputModel paymentInputModel = new()
            {
                CardName = checkoutInputModel.CardName,
                CardNumber = checkoutInputModel.CardNumber,
                Expiration = checkoutInputModel.Expiration,
                CVV = checkoutInputModel.CVV,
                TotalPrice = basket.TotalPrice
            };

            bool response = await _paymentService.ReceivePayment(paymentInputModel);

            if (response is false) return default;

            OrderCreateInput orderCreateInput = new()
            {
                BuyerId = _userProvider.GetUserId,
                AddressCreateModel = new AddressCreateModel()
                {
                    Street = checkoutInputModel.Street,
                    District = checkoutInputModel.District,
                    Province = checkoutInputModel.Province,
                    ZipCode = checkoutInputModel.ZipCode,
                    Line = checkoutInputModel.Line
                }
            };

            basket.BasketItemViewModels.ForEach(x =>
            {
                orderCreateInput.OrderItemViewModels.Add(new OrderItemViewModel()
                {
                    ProductId = x.CourseId,
                    Price= x.Price,
                    ProductName = x.CourseName,
                    PictureUrl = "asd"
                });
            });

            var result =  await _httpClient.PostAsJsonAsync<OrderCreateInput>("order/create", orderCreateInput);

            if (result.IsSuccessStatusCode is false) return default;

            SuccessResponse<OrderCreatedViewModel> successResponse = await result
                                .Content.ReadFromJsonAsync<SuccessResponse<OrderCreatedViewModel>>();

            await _basketService.Delete();

            return successResponse.Data;
        }

        public async Task<List<OrderViewModel>> GetOrders()
        {
            var response = await _httpClient.GetFromJsonAsync<SuccessResponse<List<OrderViewModel>>>("order/all");

            if (response.Success is false)
                return default;

            return response.Data;   
        }

        public async Task<OrderSuspendViewModel> SuspendOrder(CheckoutInputModel checkoutInputModel)
        {

            var basket = await _basketService.Get();

            OrderCreateInput orderCreateInput = new()
            {
                BuyerId = _userProvider.GetUserId,
                AddressCreateModel = new AddressCreateModel()
                {
                    Street = checkoutInputModel.Street,
                    District = checkoutInputModel.District,
                    Province = checkoutInputModel.Province,
                    ZipCode = checkoutInputModel.ZipCode,
                    Line = checkoutInputModel.Line
                }
            };

            basket.BasketItemViewModels.ForEach(x =>
            {
                orderCreateInput.OrderItemViewModels.Add(new OrderItemViewModel()
                {
                    ProductId = x.CourseId,
                    Price = x.Price,
                    ProductName = x.CourseName,
                    PictureUrl = "asd"
                });
            });

            PaymentInputModel paymentInputModel = new()
            {
                CardName = checkoutInputModel.CardName,
                CardNumber = checkoutInputModel.CardNumber,
                Expiration = checkoutInputModel.Expiration,
                CVV = checkoutInputModel.CVV,
                TotalPrice = basket.TotalPrice,
                OrderCreateInput= orderCreateInput
            };

            bool response = await _paymentService.ReceivePayment(paymentInputModel);

            if (response is false) return new OrderSuspendViewModel()
            {
                Error = "Fail"
            };

            await _basketService.Delete();

            return new OrderSuspendViewModel()
            {
                IsSuccessed= true,
            };
        }
    }
}
