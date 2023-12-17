using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Shipfinity.Domain.Models
{
    public class User : IdentityUser
    {
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Role { get; set; } = string.Empty;

        public Address? Address { get; set; }

        public int? AddressId { get; set; }

        public List<Order> Orders { get; set; } = new();

        public List<ReviewProduct> ReviewProducts { get; set; } = new();

        public List<PaymentInfo> PaymentInfos { get; set; } = new();

        public List<Product> Products { get; set; } = new();
    }
}
