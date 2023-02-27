namespace ProductSale.Service.Basket.Dtos
{
    public class BasketDto
    {
        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public List<BasketItemDto> BasketItemDtos { get; set; }
        public decimal TotalPrice { get => BasketItemDtos.Sum(x => x.Price * x.Quantity); }
    }
}
