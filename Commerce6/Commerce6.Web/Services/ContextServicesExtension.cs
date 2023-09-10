using Commerce6.Web.Services.Abstraction;
using Commerce6.Web.Services.AppUserServices;
using Commerce6.Web.Services.ContactServices;
using Commerce6.Web.Services.MerchantServices;
using Commerce6.Web.Services.SaleServices;

namespace Commerce6.Web.Services
{
    public static class ContextServicesExtension
    {
        public static IServiceCollection AddContextServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserService, UserService>();

            services
                .AddScoped<ICommentService, CommentService>()
                .AddScoped<IFollowService, FollowService>()
                .AddScoped<IProductReviewService, ProductReviewService>()
                .AddScoped<IShopReviewService, ShopReviewService>()
                .AddScoped<ICategoryService, CategoryService>()
                .AddScoped<IProductService, ProductService>()
                .AddScoped<IShopService, ShopService>()
                .AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}
