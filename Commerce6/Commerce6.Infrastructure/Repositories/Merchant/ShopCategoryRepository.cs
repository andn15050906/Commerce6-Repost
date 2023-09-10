using Commerce6.Data.Domain.Merchant;

namespace Commerce6.Infrastructure.Repositories.Merchant
{
    public class ShopCategoryRepository : BaseRepository<ShopCategory>
    {
        public ShopCategoryRepository(Context context) : base(context) { }

        public bool CategoryExisted(string name, int shopId) => Any(c => c.Name == name && c.ShopId == shopId);
    }
}
