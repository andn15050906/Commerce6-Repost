using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Commerce6.Infrastructure.Models.Merchant;
using Commerce6.Web.Services.Abstraction;
using Commerce6.Web.Services;
using Commerce6.Web.Models.Merchant.ShopDTOs;
using Commerce6.Web.Helpers.Session;
using Commerce6.Web.Helpers.Cookie;

namespace Commerce6.Web.Controllers.MerchantControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopsController : ControllerBase
    {
        private readonly IShopService _shopService;

        public ShopsController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            ShopDTO? result = _shopService.Get(id);

            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] CreateShopDTO dto)
        {
            StatusMessage result = await _shopService.Create(dto, HttpContext.GetUserId());

            return result switch
            {
                StatusMessage.Created => new StatusCodeResult(201),
                StatusMessage.Conflict => Conflict(),
                StatusMessage.Unauthorized => Unauthorized(),
                _ => BadRequest()
            };
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] UpdateShopDTO dto)
        {
            StatusMessage result = await _shopService.Update(dto, HttpContext.GetShopId());

            return result switch
            {
                StatusMessage.Ok => Ok(),
                StatusMessage.Unauthorized => Unauthorized(),
                _ => BadRequest()
            };
        }

        [HttpDelete]
        [Authorize]
        public IActionResult Delete()
        {
            StatusMessage result = _shopService.Delete(HttpContext.GetShopId());

            return result switch
            {
                StatusMessage.Ok => Ok(),
                StatusMessage.Unauthorized => Unauthorized(),
                _ => BadRequest()
            };
        }






        [HttpPost("/api/[controller]/Category")]
        [Authorize]
        public IActionResult CreateCategory(string name)
        {
            StatusMessage result = _shopService.CreateCategory(name, HttpContext.GetShopId());

            return result switch
            {
                StatusMessage.Created => new StatusCodeResult(201),
                StatusMessage.Conflict => Conflict(),
                StatusMessage.Unauthorized => Unauthorized(),
                _ => BadRequest()
            };
        }

        [HttpPut("/api/[controller]/Category")]
        [Authorize]
        public IActionResult UpdateCategory(int id, string name)
        {
            StatusMessage result = _shopService.UpdateCategory(id, name, HttpContext.GetShopId());

            return result switch
            {
                StatusMessage.Ok => Ok(),
                StatusMessage.Unauthorized => Unauthorized(),
                StatusMessage.Conflict => Conflict(),
                _ => BadRequest()
            };
        }

        [HttpDelete("/api/[controller]/Category")]
        [Authorize]
        public IActionResult DeleteCategory(int id)
        {
            StatusMessage result = _shopService.DeleteCategory(id, HttpContext.GetShopId());

            return result switch
            {
                StatusMessage.Ok => Ok(),
                _ => BadRequest()
            };
        }
    }
}
