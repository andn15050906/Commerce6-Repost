using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Commerce6.Infrastructure.Models.Contact;
using Commerce6.Web.Services.Abstraction;
using Commerce6.Web.Models.Merchant.ProductReviewDTOs;
using Commerce6.Web.Services;
using Commerce6.Web.Helpers.Cookie;

namespace Commerce6.Web.Controllers.ContactControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductReviewsController : ControllerBase
    {
        private readonly IProductReviewService _productReviewService;

        public ProductReviewsController(IProductReviewService productReviewService)
        {
            _productReviewService = productReviewService;
        }

        [HttpGet]
        public IActionResult Get(string productId)
        {
            return Ok(_productReviewService.Get(productId));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(CreateProductReviewDTO dto)
        {
            (ProductReviewDTO?, StatusMessage) result = _productReviewService.Create(dto, HttpContext.GetUserId());

            return result.Item2 switch
            {
                StatusMessage.Created => new StatusCodeResult(201),
                StatusMessage.Unauthorized => Unauthorized(),
                _ => BadRequest()
            };
        }

        [HttpPut]
        [Authorize]
        public IActionResult Update(UpdateProductReviewDTO dto)
        {
            (ProductReviewDTO?, StatusMessage) result = _productReviewService.Update(dto, HttpContext.GetUserId());

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
            StatusMessage result = _productReviewService.Delete(id, HttpContext.GetUserId());

            return result switch
            {
                StatusMessage.Ok => Ok(),
                StatusMessage.Unauthorized => Unauthorized(),
                _ => BadRequest()
            };
        }
    }
}
