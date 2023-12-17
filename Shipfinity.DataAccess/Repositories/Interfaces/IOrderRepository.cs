using Shipfinity.Domain.Models;

namespace Shipfinity.DataAccess.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<int> CreateAsync(Order order);
        Task<List<Order>> GetAllByUserIdAsync(string userId);
        Task<List<Order>> GetAllByProductIdAsync(int productId);
        Task<List<Order>> GetAllBySellerAsync(string sellerId);
        Task<PaymentInfo> GetMatching(string CardNumber, string ExpirationDate);
    }
}
