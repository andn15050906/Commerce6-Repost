using Commerce6.Infrastructure;
using Commerce6.Infrastructure.Repositories.AppUser;
using Commerce6.Infrastructure.Repositories.Contact;
using Commerce6.Infrastructure.Repositories.Merchant;
using Commerce6.Infrastructure.Repositories.Sale;

namespace Commerce6.Web.Services.Abstraction
{
    public interface IUnitOfWork
    {
        Context Context { get; }
        void Save();

        UserRepository UserRepo { get; }
        CommentRepository CommentRepo { get; }
        ProductRepository ProductRepo { get; }
        ShopRepository ShopRepo { get; }
        OrderRepository OrderRepo { get; }
        ProductReviewRepository ProductReviewRepo { get; }
        ShopReviewRepository ShopReviewRepo { get; }
        FollowRepository FollowRepo { get; }
        AddressRepository AddressRepo { get; }
        CategoryRepository CategoryRepo { get; }
        ShopCategoryRepository ShopCategoryRepo { get; }
        ProductAttributeRepository ProductAttributeRepo { get; }
    }
}
