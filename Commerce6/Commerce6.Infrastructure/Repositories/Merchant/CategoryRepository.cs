using Commerce6.Data.Domain.Merchant;

namespace Commerce6.Infrastructure.Repositories.Merchant
{
    public sealed class CategoryRepository : BaseRepository<Category>
    {
        public CategoryRepository(Context context) : base(context) { }

        public bool CategoryExisted(string name) => Any(c => c.Name == name);

        public Category? FindByName(string name) => DbSet.FirstOrDefault(_ => _.Name == name);
    }
}
