namespace ProductSale.Service.FakePayment.Models
{
    public class Order
    {
        public AddressDto AddressDto { get; set; }
        public string BuyerId { get; set; }
        public List<OrderItemDto> OrderItemDtos { get; set; }
}

    public class OrderItemDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string? PictureUrl { get; set; }
        public decimal Price { get; set; }
    }

    public class AddressDto 
    {
        public string Province { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string? Line { get; set; }
    }
}
