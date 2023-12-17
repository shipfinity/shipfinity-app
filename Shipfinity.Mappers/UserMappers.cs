using Shipfinity.Domain.Models;
using Shipfinity.DTOs.CustomerDTOs;

namespace Shipfinity.Mappers
{
    public static class UserMappers
    {
        public static CustomerLoginResponseDto ToCustomerLoginResponse(this User user, string token)
        {
            return new CustomerLoginResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Role = user.Role,
                Token = token
            };
        }
    }
}
