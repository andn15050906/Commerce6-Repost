using Commerce6.Web.Models.Merchant.CategoryDTOs;

namespace Commerce6.Web.Services.Abstraction
{
    public interface ICategoryService
    {
        StatusMessage Create(CreateCategoryDTO dto);
        StatusMessage Update(UpdateCategoryDTO dto);
        bool Delete(string name);
    }
}
