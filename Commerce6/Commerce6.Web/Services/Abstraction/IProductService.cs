using Commerce6.Infrastructure.Models.Common;
using Commerce6.Infrastructure.Models.Merchant;
using Commerce6.Web.Models.Merchant.ProductDTOs;

namespace Commerce6.Web.Services.Abstraction
{
    public interface IProductService
    {
        List<ProductDTO> Get(QueryProductDTO query);
        PagedResult<ProductDTO> GetPaged(QueryProductDTO query);
        ProductFullDTO? GetById(string id, int commentTake, int reviewTake);
        Task<StatusMessage> Create(CreateProductDTO dto, int? shopId);
        Task<StatusMessage> Update(UpdateProductDTO dto, int? shopId);
        StatusMessage Delete(string id, int? shopId);
    }
}
