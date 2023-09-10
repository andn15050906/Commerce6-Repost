using Commerce6.Infrastructure;
using Commerce6.Infrastructure.Repositories.AppUser;
using Commerce6.Infrastructure.Repositories.Contact;
using Commerce6.Infrastructure.Repositories.Merchant;
using Commerce6.Infrastructure.Repositories.Sale;
using Commerce6.Web.Services.Abstraction;

namespace Commerce6.Web.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;

        private UserRepository? _userRepo;
        private CommentRepository? _commentRepo;
        private ProductRepository? _productRepo;
        private ShopRepository? _shopRepo;
        private OrderRepository? _orderRepo;
        private ProductReviewRepository? _productReviewRepo;
        private ShopReviewRepository? _shopReviewRepo;
        private FollowRepository? _followRepo;
        private AddressRepository? _addressRepo;
        private CategoryRepository? _categoryRepo;
        private ShopCategoryRepository? _shopCategoryRepo;
        private ProductAttributeRepository? _productAttributeRepo;

        public UnitOfWork(Context context)
        {
            _context = context;
        }



        public Context Context { get => _context; }

        public void Save() => _context.SaveChanges();

        //Activator.CreateInstance is slow
        public UserRepository UserRepo                          { get => _userRepo ??= new UserRepository(_context); }
        public CommentRepository CommentRepo                    { get => _commentRepo ??= new CommentRepository(_context); }
        public ProductRepository ProductRepo                    { get => _productRepo ??= new ProductRepository(_context); }
        public ShopRepository ShopRepo                          { get => _shopRepo ??= new ShopRepository(_context); }
        public OrderRepository OrderRepo                        { get => _orderRepo ??= new OrderRepository(_context); }
        public ProductReviewRepository ProductReviewRepo        { get => _productReviewRepo ??= new ProductReviewRepository(_context); }
        public ShopReviewRepository ShopReviewRepo              { get => _shopReviewRepo ??= new ShopReviewRepository(_context); }
        public FollowRepository FollowRepo                      { get => _followRepo ??= new FollowRepository(_context); }
        public AddressRepository AddressRepo                    { get => _addressRepo ??= new AddressRepository(_context); }
        public CategoryRepository CategoryRepo                  { get => _categoryRepo ??= new CategoryRepository(_context); }
        public ShopCategoryRepository ShopCategoryRepo          { get => _shopCategoryRepo ??= new ShopCategoryRepository(_context); }
        public ProductAttributeRepository ProductAttributeRepo  { get => _productAttributeRepo ??= new ProductAttributeRepository(_context); }
    }
}
