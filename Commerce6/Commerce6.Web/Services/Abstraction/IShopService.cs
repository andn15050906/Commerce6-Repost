using Commerce6.Infrastructure.Models.Merchant;
using Commerce6.Web.Models.Merchant.ShopDTOs;

namespace Commerce6.Web.Services.Abstraction
{
    public interface IShopService
    {
        ShopDTO? Get(int id);
        Task<StatusMessage> Create(CreateShopDTO dto, string? userId);
        Task<StatusMessage> Update(UpdateShopDTO dto, int? shopId);
        StatusMessage Delete(int? shopId);

        StatusMessage CreateCategory(string name, int? shopId);
        StatusMessage UpdateCategory(int id, string name, int? shopId);
        StatusMessage DeleteCategory(int id, int? shopId);
    }
}
