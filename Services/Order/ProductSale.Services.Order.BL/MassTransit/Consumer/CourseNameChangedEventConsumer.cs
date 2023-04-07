using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductSale.Services.Order.DAL;
using ProductSale.Shared.Infrastructure.MassTransit.Events;

namespace ProductSale.Services.Order.BL.MassTransit.Consumer
{
    public class CourseNameChangedEventConsumer : IConsumer<CourseNameChangedEvent>
    {
        private readonly OrderDbContext _orderDbContext;

        public CourseNameChangedEventConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            var courses = await _orderDbContext.OrderItems.Where(x => x.ProductId == context.Message.CourseId).ToListAsync();

            courses.ForEach(x =>
            {
                x.UpdateOrderItem(context.Message.CourseName, x.PictureUrl, x.Price);
            });

            await _orderDbContext.SaveChangesAsync();
        }
    }
}
