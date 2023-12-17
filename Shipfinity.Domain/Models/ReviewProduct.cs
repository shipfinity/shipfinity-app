using System.ComponentModel.DataAnnotations;

namespace Shipfinity.Domain.Models
{
    public class ReviewProduct : BaseEntity
    {
        public string Comment { get; set; }
        [Required]
        public int Rating { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
