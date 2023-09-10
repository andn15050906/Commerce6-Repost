using Commerce6.Infrastructure.Models.AppUser;
using Commerce6.Infrastructure.Models.Merchant;
using Commerce6.Web.Models.AppUser.UserDTOs;
using Commerce6.Web.Services.JwtService;

namespace Commerce6.Web.Services.Abstraction
{
    public interface IUserService
    {
        UserDTO? GetCustomerInfo(string id);
        StatusMessage Register(RegisterDTO dto);
        Task<StatusMessage> Update(UpdateUserDTO dto, string? userId);
        StatusMessage ChangePassword(ChangePasswordDTO dto, string? userId);
        string? GenerateResetPasswordLink(string email, string domain);
        string? ResetPassword(string? userId, string token);
        StatusMessage Login(LoginDTO dto, JwtProvider jwtProvider, out string? accessToken, out string? refreshToken);
        StatusMessage Refresh(string? accessToken, string? refreshToken, JwtProvider jwtProvider, out string? newAccessToken, out string? newRefreshToken);
        ShopDTO? GetShop(string? userId);

        //Development
        StatusMessage RegisterAsAdmin(RegisterDTO dto);
    }
}
