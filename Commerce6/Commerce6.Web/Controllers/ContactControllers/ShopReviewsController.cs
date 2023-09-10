using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Commerce6.Infrastructure.Models.Contact;
using Commerce6.Web.Services.Abstraction;
using Commerce6.Web.Services;
using Commerce6.Web.Models.Merchant.ShopReviewDTOs;
using Commerce6.Web.Helpers.Cookie;

namespace Commerce6.Web.Controllers.ContactControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopReviewsController : ControllerBase
    {
        private readonly IShopReviewService _shopReviewService;

        public ShopReviewsController(IShopReviewService shopReviewService)
        {
            _shopReviewService = shopReviewService;
        }

        [HttpGet]
        public IActionResult Get(int shopId)
        {
            return Ok(_shopReviewService.Get(shopId));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(CreateShopReviewDTO dto)
        {
            (ShopReviewDTO?, StatusMessage) result = _shopReviewService.Create(dto, HttpContext.GetUserId());

            return result.Item2 switch
            {
                StatusMessage.Created => new StatusCodeResult(201),
                StatusMessage.Unauthorized => Unauthorized(),
                _ => BadRequest()
            };
        }

        [HttpPut]
        [Authorize]
        public IActionResult Update(UpdateShopReviewDTO dto)
        {
            (ShopReviewDTO?, StatusMessage) result = _shopReviewService.Update(dto, HttpContext.GetUserId());

            return result.Item2 switch
            {
                StatusMessage.Ok => Ok(),
                StatusMessage.Unauthorized => Unauthorized(),
                _ => BadRequest()
            };
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            StatusMessage result = _shopReviewService.Delete(id, HttpContext.GetUserId());

            return result switch
            {
                StatusMessage.Ok => Ok(),
                StatusMessage.Unauthorized => Unauthorized(),
                _ => BadRequest()
            };
        }
    }
}
