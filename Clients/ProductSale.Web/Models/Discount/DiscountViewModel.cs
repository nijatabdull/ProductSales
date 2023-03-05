namespace ProductSale.Web.Models.Discount
{
    public class DiscountViewModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public int Rate { get; set; }

        public string Code { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
