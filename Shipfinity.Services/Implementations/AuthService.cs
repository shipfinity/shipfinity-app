using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shipfinity.DataAccess.Repositories.Interfaces;
using Shipfinity.Domain.Enums;
using Shipfinity.Domain.Models;
using Shipfinity.DTOs.CustomerDTOs;
using Shipfinity.DTOs.SellerDTO_s;
using Shipfinity.DTOs.UserDTOs;
using Shipfinity.Mappers;
using Shipfinity.Services.Interfaces;
using Shipfinity.Shared.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shipfinity.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        public AuthService(UserManager<User> userManager, IConfiguration configuration, IUserRepository userRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<CustomerLoginResponseDto> LoginCustomer(UserLoginDto dto)
        {
            User customer = await _userManager.FindByNameAsync(dto.Username);
            if (customer == null || customer.Role != Roles.Customer)
            {
                throw new BadCredentialsException();
            }

            if (!await _userManager.CheckPasswordAsync(customer, dto.Password))
            {
                throw new BadCredentialsException();
            }
            string token = GenerateToken(customer);
            return customer.ToCustomerLoginResponse(token);
        }

        public async Task RegisterCustomer(CustomerRegisterDto dto)
        {
            if (string.IsNullOrEmpty(dto.Username) || string.IsNullOrEmpty(dto.Password) || string.IsNullOrEmpty(dto.Email))
            {
                throw new UserRegisterException("Username password and email are required fields");
            }

            if (dto.Password != dto.ConfirmPassword)
            {
                throw new UserRegisterException("Password does not match");
            }

            if (await _userManager.FindByNameAsync(dto.Username) != null)
            {
                throw new UserRegisterException("Username is already taken");
            }

            User customer = new User
            {
                UserName = dto.Username,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Role = Roles.Customer
            };
            await _userManager.CreateAsync(customer, dto.Password);
        }

        public async Task RegisterSeller(SellerRegisterDto dto)
        {

            if (string.IsNullOrEmpty(dto.Username) || string.IsNullOrEmpty(dto.Password) || string.IsNullOrEmpty(dto.Email))
            {
                throw new UserRegisterException("Username, password, and email are required fields");
            }

            if (dto.Password != dto.ConfirmPassword)
            {
                throw new UserRegisterException("Password does not match");
            }


            if (await _userManager.FindByNameAsync(dto.Username) != null)
            {
                throw new UserRegisterException("Username is already taken");
            }

            User seller = new User
            {
                UserName = dto.Username,
                Email = dto.Email,
                Name = dto.Name,
                Role = Roles.Seller
            };
            await _userManager.CreateAsync(seller, dto.Password);
        }
        public async Task<CustomerLoginResponseDto> LoginSeller(UserLoginDto dto)
        {
            User seller = await _userManager.FindByNameAsync(dto.Username);
            if (seller == null || (seller.Role != Roles.Seller && seller.Role != Roles.Admin))
            {
                throw new BadCredentialsException();
            }

            if (!await _userManager.CheckPasswordAsync(seller, dto.Password))
            {
                throw new BadCredentialsException();
            }
            string token = GenerateToken(seller);
            return seller.ToCustomerLoginResponse(token);
        }

        public async Task<bool> InitialAdmin(SellerRegisterDto dto)
        {
            int adminsCount = await _userRepository.CountByRole(Roles.Admin);
            if (adminsCount > 0) return false;

            if (string.IsNullOrEmpty(dto.Username) || string.IsNullOrEmpty(dto.Password) || string.IsNullOrEmpty(dto.Email))
            {
                throw new UserRegisterException("Username, password, and email are required fields");
            }


            if (await _userManager.FindByNameAsync(dto.Username) != null)
            {
                throw new UserRegisterException("Username is already taken");
            }

            User user = new User
            {
                UserName = dto.Username,
                Email = dto.Email,
                Name = dto.Name,
                Role = Roles.Admin
            };
            await _userManager.CreateAsync(user, dto.Password);
            return true;
        }

        private string GenerateToken(User user)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            Claim[] claims = new Claim[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:Expire"])),
                SigningCredentials = credentials,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
