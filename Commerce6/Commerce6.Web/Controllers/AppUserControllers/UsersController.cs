using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Commerce6.Web.Services.Abstraction;
using Commerce6.Infrastructure.Models.AppUser;
using Commerce6.Web.Services;
using Commerce6.Web.Models.AppUser.UserDTOs;
using Commerce6.Web.Services.EmailService;
using Commerce6.Web.Helpers.Cookie;

namespace Commerce6.Web.Controllers.AppUserControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult Register(RegisterDTO dto)
        {
            StatusMessage result = _userService.Register(dto);

            return result switch
            {
                StatusMessage.Created => new StatusCodeResult(201),
                _ => Conflict(),
            };
        }

        [HttpPost("/api/[controller]/ForgotPassword")]
        public IActionResult ForgotPassword(string email, [FromServices] EmailSender emailService)
        {
            //Requires front-end request path
            string? link = _userService.GenerateResetPasswordLink(email, GetAppDomain(emailService));
            if (link == null)
                return NotFound();

            string subject = "[VitaminK] Password reset link";
            string body = $"Please reset your password by <a href='{link}'>clicking here</a>.";
            emailService.SendEmailAsync(email, subject, body);
            return Ok();
        }

        [HttpPost("/api/[controller]/ResetPassword")]
        public IActionResult ResetPassword(string userId, string token)
        {
            string? generatedPassword = _userService.ResetPassword(userId, token);

            return generatedPassword != null ? Ok(generatedPassword) : NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            UserDTO? result = _userService.GetCustomerInfo(id);

            return result != null ? Ok(result) : NotFound();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] UpdateUserDTO dto)
        {
            StatusMessage result = await _userService.Update(dto, HttpContext.GetUserId());

            return result != StatusMessage.Unauthorized ? Ok() : Unauthorized();
        }

        [HttpPut("/api/[controller]/ChangePassword")]
        [Authorize]
        public IActionResult ChangePassword(ChangePasswordDTO dto)
        {
            StatusMessage result = _userService.ChangePassword(dto, HttpContext.GetUserId());

            return result switch
            {
                StatusMessage.Ok => Ok(),
                _ => Unauthorized()
            };
        }








        private string GetAppDomain(EmailSender emailService)
        {
            //if configured, use the configured domain, else use the request's domain
            string? domain = emailService.GetAppDomain();
            if (domain == null)
                domain = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            return domain;
        }






        //Development only
        [HttpPost("/api/[controller]/admin")]
        public IActionResult RegisterAsAdmin(RegisterDTO dto)
        {
            StatusMessage result = _userService.RegisterAsAdmin(dto);

            return result switch
            {
                StatusMessage.Created => new StatusCodeResult(201),
                _ => Conflict(),
            };
        }
    }
}
