
namespace ProductSale.Shared.Infrastructure.MassTransit.Events
{
    public class CourseNameChangedEvent
    {
        public string CourseId { get; set; }
        public string CourseName { get;set; }
    }
}
