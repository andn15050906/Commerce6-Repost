using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Commerce6.Data.Domain.Contact;
using Commerce6.Infrastructure.Models.Contact;
using Commerce6.Infrastructure.Models.AppUser;

namespace Commerce6.Infrastructure.Repositories.Contact
{
    public sealed class ShopReviewRepository : BaseRepository<ShopReview>
    {
        internal static readonly Expression<Func<ShopReview, ShopReviewDTO>> s_mapExpression = _ => new ShopReviewDTO()
        {
            Id = _.Id,
            Content = _.Content,
            Rating = _.Rating,
            CreatedAt = _.CreatedAt,
            ModifiedAt = _.ModifiedAt,
            Customer = new UserMinDTO { Id = _.CustomerId, FullName = _.Customer.FullName, Avatar = _.Customer.Avatar },
            ShopId = _.ShopId
        };






        public ShopReviewRepository(Context context) : base(context) { }

        public List<ShopReviewDTO> Get(int shopId)
        {
            return DbSet.Include(r => r.Customer).Where(r => r.ShopId == shopId)
                .Select(s_mapExpression)
                .OrderByDescending(r => r.ModifiedAt).ToList();
        }

        public ShopReview? GetById(int id)
            => DbSet.Include(r => r.Customer).FirstOrDefault(r => r.Id == id);
    }
}
