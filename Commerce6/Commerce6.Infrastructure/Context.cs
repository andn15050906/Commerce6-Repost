using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Commerce6.Data.Domain.AppUser;
using Commerce6.Data.Domain.Merchant;
using Commerce6.Data.Domain.Sale;
using Commerce6.Data.Domain.Contact;
using Commerce6.Data.Domain.Statistics;

namespace Commerce6.Infrastructure
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<ShopReview> ShopReviews { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAttribute> Attributes { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ShopCategory> ShopCategories { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Order_Product> Order_Products { get; set; }

        public DbSet<OrderStatistics> OrderStatistics { get; set; }
        public DbSet<ViewStatistics> ViewStatistics { get; set; }


        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            EntityTypeBuilder<User> userBuilder = builder.Entity<User>();
            userBuilder.HasIndex(u => u.Phone).IsUnique();
            userBuilder.HasIndex(u => u.Email).IsUnique();
            userBuilder.Property(u => u.Role).HasConversion(r => r.ToString(), r => (Role)Enum.Parse(typeof(Role), r));
            userBuilder.HasOne(u => u.Shop).WithOne(s => s.Owner).HasForeignKey<Shop>(s => s.OwnerId).OnDelete(DeleteBehavior.SetNull);
            userBuilder.HasMany(u => u.Followed).WithMany(s => s.Followers)
                .UsingEntity<Follow>(f => {
                    //Cascade path: User -> Shop -> Follow
                    f.HasOne(f => f.User).WithMany().HasForeignKey(f => f.UserId).OnDelete(DeleteBehavior.NoAction);
                    f.HasOne(f => f.Shop).WithMany().HasForeignKey(f => f.ShopId);
                });

            builder.Entity<Shop>().HasOne(s => s.Owner).WithOne(o => o.Shop).HasForeignKey<User>(u => u.ShopId).IsRequired(false);
            //Shop -> Product (Shop cannot be null) -> ShopCategory
            builder.Entity<ShopCategory>().HasOne(c => c.Shop).WithMany().OnDelete(DeleteBehavior.SetNull);

            EntityTypeBuilder<Product> productBuilder = builder.Entity<Product>();
            productBuilder.HasMany(p => p.Images).WithOne(i => i.Product);
            productBuilder.HasOne(p => p.ThumbImage).WithOne().HasForeignKey<Product>(p => p.ThumbImageId).IsRequired(false);

            builder.Entity<Order>().Property(o => o.State).HasConversion(s => s.ToString(), s => (OrderState)Enum.Parse(typeof(OrderState), s));
            builder.Entity<Order_Product>().HasKey(op => new { op.OrderId, op.ProductId });
        }
    }
}