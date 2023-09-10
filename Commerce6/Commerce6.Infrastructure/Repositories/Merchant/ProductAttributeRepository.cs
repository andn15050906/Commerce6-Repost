using Commerce6.Data.Domain.Merchant;

namespace Commerce6.Infrastructure.Repositories.Merchant
{
    public sealed class ProductAttributeRepository : BaseRepository<ProductAttribute>
    {
        public ProductAttributeRepository(Context context) : base(context) { }

        public void RemoveRangeById(string productId, int[] ids)
            => DbSet.RemoveRange(DbSet.Where(a => ids.Contains(a.Id) && a.ProductId == productId));
    }
}
