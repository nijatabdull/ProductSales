using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductSale.Services.Order.BL.Dtos;
using ProductSale.Services.Order.BL.Queries;
using ProductSale.Services.Order.BL.Statics;
using ProductSale.Services.Order.DAL;
using ProductSale.Shared.Infrastructure.Response;
using ProductSale.Shared.Infrastructure.Response.Base;

namespace ProductSale.Services.Order.BL.Handlers
{
    public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery,Response>
    {
        private readonly OrderDbContext _orderDbContext;

        public GetOrdersByUserIdQueryHandler(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        async Task<Response> IRequestHandler<GetOrdersByUserIdQuery, Response>.Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            List<Core.Models.Order> orders = await _orderDbContext.Orders.Include(x => x.OrderItems)
                .Where(x => x.BuyerId == request.UserId).ToListAsync();

            //if(orders is null || orders.Any() is false)
            //{
            //    return new ErrorResponse("Order is not found");
            //}

            List<OrderDto> orderDtos = ObjectMapper.Mapper.Map<List<OrderDto>>(orders);

            return new SuccessResponse<List<OrderDto>>(orderDtos);
        }
    }
}
