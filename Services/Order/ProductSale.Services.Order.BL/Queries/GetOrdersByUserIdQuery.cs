using MediatR;
using ProductSale.Shared.Infrastructure.Response.Base;

namespace ProductSale.Services.Order.BL.Queries
{
    public class GetOrdersByUserIdQuery : IRequest<Response>
    {
        public string UserId { get; set; }
    }
}
