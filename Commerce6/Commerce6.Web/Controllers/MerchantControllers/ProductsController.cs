using Commerce6.Infrastructure.Models.Merchant;
using Commerce6.Web.Services.Abstraction;
using Commerce6.Web.Helpers.Session;
using Commerce6.Web.Models.Merchant.ProductDTOs;
using Commerce6.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Commerce6.Web.Controllers.MerchantControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // not friendly paging
        [HttpGet]
        [ResponseCache(Duration = 60)]
        public IActionResult Get([FromQuery] QueryProductDTO dto)
        {
            return Ok(_productService.Get(dto));
        }

        // added paging param to avoid duplicated params
        [HttpGet("/paging")]
        [ResponseCache(Duration = 60)]
        public IActionResult Get([FromQuery] QueryProductDTO dto, bool paging = true)
        {
            return Ok(_productService.GetPaged(dto));
        }

        [HttpGet("{id}")]
        [ResponseCache(Duration = 60)]
        public IActionResult Get(string id, int commentTake = 5, int reviewTake = 3)
        {
            ProductFullDTO? result = _productService.GetById(id, commentTake, reviewTake);

            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductDTO dto)
        {
            StatusMessage result = await _productService.Create(dto, HttpContext.GetShopId());

            return result switch
            {
                StatusMessage.Created => new StatusCodeResult(201),
                StatusMessage.Unauthorized => Unauthorized(),
                _ => BadRequest()
            };
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateProductDTO dto)
        {
            StatusMessage result = await _productService.Update(dto, HttpContext.GetShopId());

            return result switch
            {
                StatusMessage.Ok => Ok(),
                StatusMessage.Unauthorized => Unauthorized(),
                _ => BadRequest()
            };
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            StatusMessage result = _productService.Delete(id, HttpContext.GetShopId());

            return result switch
            {
                StatusMessage.Ok => Ok(),
                StatusMessage.Unauthorized => Unauthorized(),
                _ => BadRequest()
            };
        }
    }
}
