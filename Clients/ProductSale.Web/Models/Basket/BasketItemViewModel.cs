namespace ProductSale.Web.Models.Basket
{
    public class BasketItemViewModel
    {
        public int Quantity { get; set; }
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountAppliedPrice { get; set; }

    }
}
