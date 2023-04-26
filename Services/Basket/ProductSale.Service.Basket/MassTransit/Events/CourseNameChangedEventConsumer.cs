using MassTransit;
using ProductSale.Service.Basket.Services.Abstractions;
using ProductSale.Shared.Infrastructure.MassTransit.Events;

namespace ProductSale.Service.Basket.MassTransit.Events
{
    public class CourseNameChangedEventConsumer : IConsumer<CourseNameChangedEvent>
    {
        private readonly IBasketService _basketService;
        public CourseNameChangedEventConsumer(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            _basketService.ChangeCourseName(new Dtos.CourseNameChangeDto()
            {
                CourseId = context.Message.CourseId,
                CourseName = context.Message.CourseName,
            });
        }
    }
}
