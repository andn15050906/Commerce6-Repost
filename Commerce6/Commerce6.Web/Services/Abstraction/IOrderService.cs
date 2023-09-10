using Commerce6.Infrastructure.Models.Sale;
using Commerce6.Web.Models.Sale.OrderDTOs;

namespace Commerce6.Web.Services.Abstraction
{
    public interface IOrderService
    {
        List<OrderDTO>? GetAsCustomer(string? userId);
        List<OrderDTO>? GetAsMerchant(int? shopId);
        StatusMessage Create(CreateOrderDTO dto, string? customerId);
        StatusMessage Update(MerchantUpdateOrderDTO dto, int? shopId);
        StatusMessage Update(CustomerUpdateOrderDTO dto, string? customerId);
    }
}
