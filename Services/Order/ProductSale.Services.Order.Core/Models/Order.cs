using ProductSale.Services.Order.Core.Abstractions.ModelRules;

namespace ProductSale.Services.Order.Core.Models
{
    //EF Core Features
    // --Owned Types
    // --Shadow Prpperty
    // --Backing Field
    public class Order : Entity, IAggregateRoot
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public Address Address { get; set; }
        public string BuyerId { get; set; }
        private List<OrderItem> orderItems;

        public IReadOnlyCollection<OrderItem> OrderItems { get => orderItems ??= new List<OrderItem>(); }

        public Order()
        {

        }
        public Order(Address address, string buyerId)
        {
            orderItems = new List<OrderItem>();
            CreateDate = DateTime.Now;
            Address = address;
            BuyerId = buyerId;
        }

        public void AddOrderItem(string productId, string productName, string pictureUrl, decimal price)
        {
            bool exist = orderItems.Any(x => x.ProductId == productId);

            if (exist is false)
            {
                OrderItem orderItem = new OrderItem(productId, productName, pictureUrl, price);

                orderItems.Add(orderItem);
            }
        }

        public decimal GetTotalPrice => orderItems.Sum(x => x.Price);
    }
}
