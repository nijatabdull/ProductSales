using MediatR;
using ProductSale.Services.Order.BL.Dtos;
using ProductSale.Shared.Infrastructure.Response.Base;

namespace ProductSale.Services.Order.BL.Commands
{
    public class CreateOrderCommand : IRequest<Response>
    {
        public string BuyerId { get; set; }
        public List<OrderItemDto> OrderItemDtos { get; set; }
        public AddressDto AddressDto { get; set; }
    }
}
