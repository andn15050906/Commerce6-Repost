using Microsoft.AspNetCore.Mvc;
using Commerce6.Infrastructure.Models.Merchant;
using Commerce6.Web.Services.Abstraction;
using Commerce6.Web.Models.AppUser.UserDTOs;
using Commerce6.Web.Services;
using Commerce6.Web.Services.JwtService;
using Commerce6.Web.Helpers.Session;
using Commerce6.Web.Helpers.Cookie;

namespace Commerce6.Web.Controllers.AppUserControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// send info for the first time
        /// </summary>
        [HttpPost("/api/[controller]/LoginMerchant")]
        public IActionResult LoginAsMerchant()
        {
            ShopDTO? dto = _userService.GetShop(HttpContext.GetUserId());
            if (dto != null)
            {
                HttpContext.SetShopId(dto.Id);
                return Ok(dto);
            }
            return Forbid();
        }

        [HttpPost("/api/[controller]/Login")]
        public IActionResult Login([FromBody] LoginDTO dto, [FromServices] JwtProvider jwtProvider)
        {
            StatusMessage result = _userService.Login(dto, jwtProvider,
                out string? accessToken, out string? refreshToken);

            if (result == StatusMessage.Unauthorized)
                return Unauthorized();
            if (result == StatusMessage.Forbidden)
                return Forbid();

            SetCookie(accessToken!, refreshToken!);
            return Ok();
        }

        [HttpPost("/api/[controller]/Logout")]
        public void Logout()
        {
            ExpiresCookie();
        }

        [HttpPost("/api/[controller]/Refresh")]
        public IActionResult Refresh([FromServices] JwtProvider jwtProvider)
        {
            string? accessToken = HttpContext.GetAccessToken();
            string? refreshToken = HttpContext.GetRefreshToken();

            StatusMessage result = _userService.Refresh(accessToken, refreshToken, jwtProvider,
                out string? newAccessToken, out string? newRefreshToken);
            if (result == StatusMessage.Unauthorized)
                return Unauthorized();
            SetCookie(newAccessToken!, newRefreshToken!);
            return Ok();
        }






        private void SetCookie(string accessToken, string refreshToken)
        {
            CookieOptions options = CookieHelper.GetOptions();
            HttpContext.SetAccessToken(accessToken, options);
            HttpContext.SetRefreshToken(refreshToken, options);
        }

        private void ExpiresCookie()
        {
            CookieOptions options = CookieHelper.GetExpiredOptions();
            HttpContext.SetAccessToken("", options);
            HttpContext.SetRefreshToken("", options);
        }
    }
}