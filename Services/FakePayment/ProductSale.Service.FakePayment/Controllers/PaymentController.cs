using MassTransit;
using Microsoft.AspNetCore.Mvc;
using ProductSale.Service.FakePayment.Models;
using ProductSale.Shared.Infrastructure.MassTransit.Commands;

namespace ProductSale.Service.FakePayment.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PaymentController : ControllerBase
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public PaymentController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> ReceivePayment(Payment payment)
        {
            ISendEndpoint sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order"));

            CreateOrderMessageCommand createOrderMessageCommand = new()
            {
                BuyerId = payment.Order.BuyerId,
                Address = new Address()
                {
                    Street = payment.Order.AddressDto.Street,
                    District = payment.Order.AddressDto.District,
                    Line = payment.Order.AddressDto.Line,
                    Province = payment.Order.AddressDto.Province,
                    ZipCode = payment.Order.AddressDto.ZipCode,
                },
                OrderItems = new List<OrderItem>()
            };

            payment.Order.OrderItemDtos.ForEach(d =>
            {
                createOrderMessageCommand.OrderItems.Add(new OrderItem() 
                {
                      PictureUrl= d.PictureUrl,
                      Price= d.Price,
                      ProductId= d.ProductId,
                      ProductName= d.ProductName,
                });
            });

            await sendEndpoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);

            return Ok();
        }
    }
}
