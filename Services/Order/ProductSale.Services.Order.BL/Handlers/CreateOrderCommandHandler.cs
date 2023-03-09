using MediatR;
using ProductSale.Services.Order.BL.Commands;
using ProductSale.Services.Order.BL.Dtos;
using ProductSale.Services.Order.Core.Models;
using ProductSale.Services.Order.DAL;
using ProductSale.Shared.Infrastructure.Response;
using ProductSale.Shared.Infrastructure.Response.Base;

namespace ProductSale.Services.Order.BL.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response>
    {
        private readonly OrderDbContext _orderDbContext;

        public CreateOrderCommandHandler(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task<Response> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Address address = new(request.AddressDto.Province, request.AddressDto.District,
                            request.AddressDto.Street, request.AddressDto.ZipCode, request.AddressDto.Line);

                Core.Models.Order order = new(address, request.BuyerId);

                request.OrderItemDtos.ForEach(x =>
                {
                    order.AddOrderItem(x.ProductId, x.ProductName, x.PictureUrl, x.Price);
                });

                await _orderDbContext.Orders.AddAsync(order);

                await _orderDbContext.SaveChangesAsync();

                return new SuccessResponse<CreatedOrderDto>(new CreatedOrderDto() { OrderId = order.Id });
            }
            catch (Exception exp)
            {
                throw;
            }
        }
    }
}
