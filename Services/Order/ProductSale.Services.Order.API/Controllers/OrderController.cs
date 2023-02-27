using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductSale.Services.Order.API.Controllers.Base;
using ProductSale.Services.Order.BL.Commands;
using ProductSale.Services.Order.BL.Queries;
using ProductSale.Shared.Services.Abstractions;

namespace ProductSale.Services.Order.API.Controllers
{
    public class OrderController : AncestorController
    {
        private readonly IMediator _mediator;
        private readonly IUserProvider _userProvider;

        public OrderController(IMediator mediator, IUserProvider userProvider)
        {
            _mediator = mediator;
            _userProvider = userProvider;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
           return await ResultAsync(_mediator.Send(new GetOrdersByUserIdQuery() { UserId = _userProvider.GetUserId }));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderCommand createOrderCommand)
        {
            return await ResultAsync(_mediator.Send(createOrderCommand));
        }
    }
}
