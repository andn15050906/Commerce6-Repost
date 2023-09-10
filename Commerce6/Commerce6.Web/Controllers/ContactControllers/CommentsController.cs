using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Commerce6.Infrastructure.Models.Contact;
using Commerce6.Web.Helpers.Session;
using Commerce6.Web.Models.Contact.CommentDTOs;
using Commerce6.Web.Services;
using Commerce6.Web.Services.Abstraction;
using Commerce6.Web.Helpers.Cookie;

namespace Commerce6.Web.Controllers.ContactControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] QueryCommentDTO dto)
        {
            return Ok(_commentService.Get(dto));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] CreateCommentDTO dto)
        {
            (CommentDTO?, StatusMessage) result = await _commentService.Create(dto, HttpContext.GetUserId(), HttpContext.GetShopId());

            return result.Item2 switch
            {
                StatusMessage.Created => Ok(result.Item1),
                StatusMessage.Unauthorized => Unauthorized(),
                _ => BadRequest()
            };
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] UpdateCommentDTO dto)
        {
            (CommentDTO?, StatusMessage) result = await _commentService.Update(dto, HttpContext.GetUserId(), HttpContext.GetShopId());

            return result.Item2 switch
            {
                StatusMessage.Ok => Ok(result.Item1),
                StatusMessage.Unauthorized => Unauthorized(),
                _ => BadRequest()
            };
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            StatusMessage result = _commentService.Delete(id, HttpContext.GetUserId(), HttpContext.GetShopId());

            return result switch
            {
                StatusMessage.Ok => Ok(),
                StatusMessage.Unauthorized => Unauthorized(),
                _ => BadRequest()
            };
        }
    }
}
