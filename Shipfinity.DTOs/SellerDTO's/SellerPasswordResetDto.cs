namespace Shipfinity.DTOs.SellerDTO_s
{
    public class SellerPasswordResetDto
    {
        public string SellerId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
