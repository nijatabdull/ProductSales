namespace ProductSale.Services.Order.BL.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public AddressDto Address { get; set; }
        public string BuyerId { get; set; }
        public List<OrderItemDto> OrderItemDtos;
    }
}
