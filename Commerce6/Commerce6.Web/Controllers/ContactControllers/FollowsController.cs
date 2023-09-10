using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Commerce6.Web.Services;
using Commerce6.Web.Services.Abstraction;
using Commerce6.Web.Helpers.Cookie;

namespace Commerce6.Web.Controllers.ContactControllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class FollowsController : ControllerBase
    {
        private readonly IFollowService _followService;

        public FollowsController(IFollowService followService)
        {
            _followService = followService;
        }

        [HttpPost]
        public IActionResult Create(int shopId)
        {
            StatusMessage result = _followService.Create(HttpContext.GetUserId(), shopId);

            return result switch
            {
                StatusMessage.Created => new StatusCodeResult(201),
                StatusMessage.Unauthorized => Unauthorized(),
                _ => BadRequest()
            };
        }

        [HttpDelete("{shopId}")]
        public IActionResult Delete(int shopId)
        {
            StatusMessage result = _followService.Delete(HttpContext.GetUserId(), shopId);

            return result switch
            {
                StatusMessage.Ok => Ok(),
                StatusMessage.Unauthorized => Unauthorized(),
                _ => BadRequest()
            };
        }
    }
}
