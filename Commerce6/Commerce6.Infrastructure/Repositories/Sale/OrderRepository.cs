using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Commerce6.Data.Domain.Sale;
using Commerce6.Infrastructure.Models.Sale;
using Commerce6.Infrastructure.Models.AppUser;
using Commerce6.Infrastructure.Models.Contact;
using Commerce6.Infrastructure.Models.Merchant;

namespace Commerce6.Infrastructure.Repositories.Sale
{
    public sealed class OrderRepository : BaseRepository<Order>
    {
        internal static readonly Expression<Func<Order, OrderDTO>> s_mapExpression = _ => new OrderDTO
        {
            Id = _.Id,
            PaymentMethod = _.PaymentMethod,
            Transporter = _.Transporter,
            State = _.State.ToString(),
            CreatedTime = _.CreatedTime,
            CompletedTime = _.CompletedTime,
            Fee = _.Fee,
            Price = _.Price,
            Discount = _.Discount,
            Customer = new UserMinDTO
            {
                Id = _.Customer.Id,
                FullName = _.Customer.FullName,
                Avatar = _.Customer.Avatar
            },
            FromAddress = new AddressResponseDTO
            {
                Province = _.FromAddress.Province,
                District = _.FromAddress.District,
                Street = _.FromAddress.Street,
                StreetNumber = _.FromAddress.StreetNumber
            },
            ToAddress = new AddressResponseDTO
            {
                Province = _.ToAddress.Province,
                District = _.ToAddress.District,
                Street = _.ToAddress.Street,
                StreetNumber = _.ToAddress.StreetNumber
            },
            Shop = new ShopMinDTO
            {
                Id = _.Shop.Id,
                Name = _.Shop.Name,
                Avatar = _.Shop.Avatar
            },
            Order_Products = _.Order_Products.Select(op => new Order_ProductDTO
            {
                Product = new ProductMinDTO
                {
                    Id = op.ProductId,
                    Price = op.Product.Price,
                    Name = op.Product.Name,
                    Discount = op.Product.Discount
                },
                Quantity = op.Quantity
            })
        };






        public OrderRepository(Context context) : base(context) { }

        //filter: client-side

        //NOT completed ?
        //not including user info
        public List<OrderDTO> Get(string userId)
        {
            return DbSet.Include(o => o.FromAddress).Include(o => o.ToAddress).Include(o => o.Shop)
                .Where(o => o.CustomerId == userId)
                .Select(s_mapExpression)
                .OrderByDescending(o => o.CompletedTime).ToList();
        }

        //not including shop info
        public List<OrderDTO> Get(int shopId)
        {
            return DbSet.Include(o => o.FromAddress).Include(o => o.ToAddress).Include(o => o.Customer)
                .Where(o => o.ShopId == shopId)
                .Select(s_mapExpression)
                .OrderByDescending(o => o.CompletedTime).ToList();
        }

        public Order? GetByIdMinimum(string orderId)
            => DbSet.Include(o => o.FromAddress).Include(o => o.Shop).FirstOrDefault(o => o.Id == orderId);
    }
}
