using Microsoft.EntityFrameworkCore;
using Shipfinity.DataAccess.Context;
using Shipfinity.DataAccess.Repositories.Interfaces;

namespace Shipfinity.DataAccess.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> CountByRole(string role)
        {
            return await _context.Users.CountAsync(u => u.Role == role);
        }
    }
}
