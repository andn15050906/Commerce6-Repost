using Commerce6.Infrastructure.Models.Contact;
using Commerce6.Web.Models.Merchant.ShopReviewDTOs;

namespace Commerce6.Web.Services.Abstraction
{
    public interface IShopReviewService
    {
        List<ShopReviewDTO> Get(int shopId);
        (ShopReviewDTO?, StatusMessage) Create(CreateShopReviewDTO dto, string? userId);
        (ShopReviewDTO?, StatusMessage) Update(UpdateShopReviewDTO dto, string? userId);
        StatusMessage Delete(int id, string? userId);
    }
}
