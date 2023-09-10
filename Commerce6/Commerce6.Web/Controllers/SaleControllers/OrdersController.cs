using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Commerce6.Infrastructure.Models.Sale;
using Commerce6.Web.Services.Abstraction;
using Commerce6.Web.Services;
using Commerce6.Web.Helpers.Session;
using Commerce6.Web.Models.Sale.OrderDTOs;
using Commerce6.Web.Helpers.Cookie;

namespace Commerce6.Web.Controllers.SaleControllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("/api/[controller]/merchant")]
        public IActionResult GetAsMerchant()
        {
            List<OrderDTO>? result = _orderService.GetAsMerchant(HttpContext.GetShopId());

            return result == null ? Unauthorized() : Ok(result);
        }

        [HttpGet("/api/[controller]/customer")]
        public IActionResult GetAsCustomer()
        {
            List<OrderDTO>? result = _orderService.GetAsCustomer(HttpContext.GetUserId());

            return result == null ? Unauthorized() : Ok(result);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderDTO dto)
        {
            StatusMessage result = _orderService.Create(dto, HttpContext.GetUserId());

            return result switch
            {
                StatusMessage.Created => new StatusCodeResult(201),
                StatusMessage.Unauthorized => Unauthorized(),
                _ => BadRequest()
            };
        }

        [HttpPut("/api/[controller]/customer")]
        public IActionResult Update(CustomerUpdateOrderDTO dto)
        {
            StatusMessage result = _orderService.Update(dto, HttpContext.GetUserId());

            return result switch
            {
                StatusMessage.Ok => Ok(),
                StatusMessage.Unauthorized => Unauthorized(),
                _ => BadRequest()
            };
        }

        [HttpPut("/api/[controller]/merchant")]
        public IActionResult Update(MerchantUpdateOrderDTO dto)
        {
            StatusMessage result = _orderService.Update(dto, HttpContext.GetShopId());

            return result switch
            {
                StatusMessage.Ok => Ok(),
                StatusMessage.Unauthorized => Unauthorized(),
                _ => BadRequest()
            };
        }
    }
}
