using MassTransit;
using ProductSale.Services.Order.DAL;
using ProductSale.Shared.Infrastructure.MassTransit.Commands;

namespace ProductSale.Services.Order.BL.MassTransit.Consumer
{
    public class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommand>
    {
        private readonly OrderDbContext _orderDbContext;

        public CreateOrderMessageCommandConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
        {
            Core.Models.Address address = new(context.Message.Address.Province,
                                                context.Message.Address.District,
                                                context.Message.Address.Street,
                                                context.Message.Address.ZipCode,
                                                context.Message.Address.Line);

            Core.Models.Order order = new Core.Models.Order(address, context.Message.BuyerId);

            context.Message.OrderItems.ForEach(item =>
            {
                order.AddOrderItem(item.ProductId, item.ProductName, item.PictureUrl, item.Price);
            });

            await _orderDbContext.Orders.AddAsync(order);

            await _orderDbContext.SaveChangesAsync();
        }
    }
}
