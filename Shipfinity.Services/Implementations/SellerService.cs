using Microsoft.AspNetCore.Identity;
using Shipfinity.DataAccess.Repositories.Interfaces;
using Shipfinity.Domain.Models;
using Shipfinity.DTOs.SellerDTO_s;
using Shipfinity.Services.Helpers;
using Shipfinity.Services.Interfaces;
using Shipfinity.Shared.Exceptions;

namespace Shipfinity.Services.Implementations
{
    public class SellerService : ISellerService
    {
        private readonly UserManager<User> _userManager;
        public SellerService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task ResetPasswordAsync(SellerPasswordResetDto passwordResetDto)
        {
            if (passwordResetDto.NewPassword != passwordResetDto.ConfirmNewPassword)
                throw new BadRequestException("New password and confirmation do not match.");

            var seller = await _userManager.FindByIdAsync(passwordResetDto.SellerId);
            if (seller == null)
                throw new SellerNotFoundException(passwordResetDto.SellerId);

            if (!await _userManager.CheckPasswordAsync(seller, passwordResetDto.OldPassword))
                throw new BadRequestException("Old password is incorrect.");

            await _userManager.ChangePasswordAsync(seller, passwordResetDto.OldPassword, passwordResetDto.NewPassword);
        }
    }
}
