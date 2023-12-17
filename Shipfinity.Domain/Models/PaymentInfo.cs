namespace Shipfinity.Domain.Models
{
    public class PaymentInfo: BaseEntity
    {
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpirationDate { get; set; }
        public List<Order> Orders { get; set; } = new();
        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}
