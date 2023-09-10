using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Commerce6.Web.Models.Merchant.CategoryDTOs;
using Commerce6.Web.Services.Abstraction;
using Commerce6.Web.Services;

namespace Commerce6.Web.Controllers.MerchantControllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryDTO dto)
        {
            StatusMessage result = _categoryService.Create(dto);

            return result switch
            {
                StatusMessage.Created => new StatusCodeResult(201),
                StatusMessage.Conflict => Conflict(),
                _ => BadRequest()
            };
        }

        [HttpPut]
        public IActionResult Update(UpdateCategoryDTO dto)
        {
            StatusMessage result = _categoryService.Update(dto);

            return result switch
            {
                StatusMessage.Ok => Ok(),
                _ => BadRequest()
            };
        }

        [HttpDelete("{name}")]
        public IActionResult Delete(string name)
        {
            return _categoryService.Delete(name) ? Ok() : BadRequest();
        }
    }
}
