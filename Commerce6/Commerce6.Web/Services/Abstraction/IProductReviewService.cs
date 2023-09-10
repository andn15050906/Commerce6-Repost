using Commerce6.Infrastructure.Models.Contact;
using Commerce6.Web.Models.Merchant.ProductReviewDTOs;

namespace Commerce6.Web.Services.Abstraction
{
    public interface IProductReviewService
    {
        List<ProductReviewDTO> Get(string productId);
        (ProductReviewDTO?, StatusMessage) Create(CreateProductReviewDTO dto, string? userId);
        (ProductReviewDTO?, StatusMessage) Update(UpdateProductReviewDTO dto, string? userId);
        StatusMessage Delete(int id, string? userId);
    }
}
