namespace Web.Domain.Entites
{

    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string PaymentMethod { get; set; }
        public string StripePaymentIntentId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}