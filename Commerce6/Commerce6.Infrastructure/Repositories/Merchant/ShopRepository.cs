using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

using Commerce6.Data.Domain.Merchant;
using Commerce6.Infrastructure.Models.Contact;
using Commerce6.Infrastructure.Models.Merchant;

namespace Commerce6.Infrastructure.Repositories.Merchant
{
    public sealed class ShopRepository : BaseRepository<Shop>
    {
        internal static readonly Expression<Func<Shop, ShopDTO>> s_mapExpression = _ => new ShopDTO
        {
            Id = _.Id,
            Name = _.Name,
            Avatar = _.Avatar,
            Phone = _.Phone,
            BankAccount = _.BankAccount,
            CreatedAt = _.CreatedAt,
            LastSeen = _.LastSeen,
            ProductCount = _.ProductCount,
            FollowerCount = _.FollowerCount,
            RatingCount = _.RatingCount,
            TotalRating = _.TotalRating,
            Address = new AddressResponseDTO
            {
                Province = _.Address!.Province,
                District = _.Address!.District,
                Street = _.Address!.Street,
                StreetNumber = _.Address!.StreetNumber
            }
        };






        public ShopRepository(Context context) : base(context) { }

        //should be authorized for that user only
        public ShopDTO? Get(string userId)
        {
            ShopDTO[] result = DbSet.Include(s => s.Address)
                .Where(s => s.OwnerId == userId)
                .Take(1)
                .Select(s_mapExpression).ToArray();
            if (result.Length == 0)
                return null;
            return result[0];
        }

        public ShopDTO? Get(int id)
        {
            ShopDTO[] result = DbSet.Include(s => s.Address)
                .Where(s => s.Id == id)
                .Take(1)
                .Select(s_mapExpression).ToArray();
            if (result.Length == 0)
                return null;
            return result[0];
        }

        public void LoadAddress(Shop shop) => Context.Entry(shop).Reference(s => s.Address).Load();

        public int? GetAddressId(int shopId)
        {
            int?[] result = DbSet.Where(u => u.Id == shopId).Take(1).Select(u => u.AddressId).ToArray();
            return result.Length > 0 ? result[0] : null;
        }
    }
}
