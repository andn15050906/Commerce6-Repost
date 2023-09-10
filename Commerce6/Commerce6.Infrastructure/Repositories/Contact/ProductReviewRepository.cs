using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Commerce6.Data.Domain.Contact;
using Commerce6.Infrastructure.Models.Contact;
using Commerce6.Infrastructure.Models.AppUser;

namespace Commerce6.Infrastructure.Repositories.Contact
{
    public sealed class ProductReviewRepository : BaseRepository<ProductReview>
    {
        internal static readonly Expression<Func<ProductReview, ProductReviewDTO>> s_mapExpression = _ => new ProductReviewDTO()
        {
            Id = _.Id,
            Content = _.Content,
            Rating = _.Rating,
            CreatedAt = _.CreatedAt,
            ModifiedAt = _.ModifiedAt,
            Customer = new UserMinDTO { Id = _.CustomerId, FullName = _.Customer.FullName, Avatar = _.Customer.Avatar },
            ProductId = _.ProductId
        };

        //requires loaded Customer
        internal static readonly Func<ProductReview, ProductReviewDTO> s_mapFunc = _ => new ProductReviewDTO()
        {
            Id = _.Id,
            Content = _.Content,
            Rating = _.Rating,
            CreatedAt = _.CreatedAt,
            ModifiedAt = _.ModifiedAt,
            Customer = new UserMinDTO { Id = _.CustomerId, FullName = _.Customer.FullName, Avatar = _.Customer.Avatar },
            ProductId = _.ProductId
        };






        public ProductReviewRepository(Context context) : base(context) { }

        public List<ProductReviewDTO> Get(string productId)
        {
            return DbSet.Include(r => r.Customer).Where(r => r.ProductId == productId)
                .Select(s_mapExpression)
                .OrderByDescending(r => r.ModifiedAt).ToList();
        }

        public ProductReview? GetById(int id)
            => DbSet.Include(r => r.Customer).FirstOrDefault(r => r.Id == id);
    }
}
