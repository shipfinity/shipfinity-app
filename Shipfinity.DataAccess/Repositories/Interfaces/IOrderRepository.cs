﻿using Shipfinity.Domain.Models;

namespace Shipfinity.DataAccess.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<List<Order>> GetAllByUserIdAsync(int userId);
        Task<List<Order>> GetAllByProductIdAsync(int productId);
    }
}
