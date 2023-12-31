﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shipfinity.DTOs.SellerDTO_s;
using Shipfinity.Services.Interfaces;
using Shipfinity.Shared.Exceptions;
using System.Security.Claims;

namespace Shipfinity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly ISellerService _sellerService;
        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        [HttpPost("ResetPassword")]
        [Authorize]
        public async Task<IActionResult> ResetPassword([FromBody] SellerPasswordResetDto passwordResetDto)
        {
            try
            {
                string userId = User.FindFirstValue("id");
                passwordResetDto.SellerId = userId;
                await _sellerService.ResetPasswordAsync(passwordResetDto);
                return Ok("Password successfully reset.");
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
