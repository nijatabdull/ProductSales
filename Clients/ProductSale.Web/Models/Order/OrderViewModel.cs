namespace ProductSale.Web.Models.Order
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string BuyerId { get; set; }
        public List<OrderItemViewModel> OrderItemViewModels;
    }
}
