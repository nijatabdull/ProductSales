namespace ProductSale.Service.FakePayment.Models
{
    public class Payment
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public decimal TotalPrice { get; set; }
        public Order Order { get; set; }
    }
}
