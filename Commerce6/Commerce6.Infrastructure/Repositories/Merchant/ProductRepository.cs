using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

using Commerce6.Data.Domain.Merchant;
using Commerce6.Infrastructure.Models.Merchant;
using Commerce6.Infrastructure.Repositories.Contact;
using Commerce6.Infrastructure.Models.Common;

namespace Commerce6.Infrastructure.Repositories.Merchant
{
    public sealed class ProductRepository : BaseRepository<Product>
    {
        internal static readonly Expression<Func<Product, ProductFullDTO>> s_mapExpression = _ => new ProductFullDTO
        {
            Id = _.Id,
            Name = _.Name,
            Price = _.Price,
            Description = _.Description,
            Discount = _.Discount,
            Stock = _.Stock,
            Sold = _.Sold,
            CreatedAt = _.CreatedAt,
            UpdatedAt = _.UpdatedAt,
            ServerTag = _.ServerTag,
            ViewCount = _.ViewCount,
            RatingCount = _.RatingCount,
            TotalRating = _.TotalRating,
            Shop = new() { Id = _.ShopId, Name = _.Shop.Name, Avatar = _.Shop.Avatar },
            ShopCategoryName = _.ShopCategory.Name,
            Attributes = _.Attributes.Select(a => new AttributeDTO { Name = a.Name, Value = a.Value }),
            Images = _.Images.Select(i => new ProductImageDTO { Image = i.Image, Position = i.Position }),
            Category = _.Category
        };

        internal static readonly Expression<Func<Product, ProductDTO>> s_mapExpressionMin = _ => new ProductDTO
        {
            Id = _.Id,
            Name = _.Name,
            Price = _.Price,
            Description = _.Description,
            Discount = _.Discount,
            Stock = _.Stock,
            Sold = _.Sold,
            CreatedAt = _.CreatedAt,
            UpdatedAt = _.UpdatedAt,
            ServerTag = _.ServerTag,
            RatingCount = _.RatingCount,
            TotalRating = _.TotalRating,
            ShopId = _.ShopId,
            ThumbImage = _.ThumbImage.Image
        };






        /*private static readonly Func<Context, string, IEnumerable<ProductFullDTO>> CompiledGetById = EF.CompileQuery(
            (Context context, string id) =>
            context.Products
                .Include(p => p.Shop)
                .Include(p => p.ShopCategory)
                .Include(p => p.Attributes)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Where(p => p.Id == id)
                .Select(MapExpression)
        );*/

        public ProductRepository(Context context) : base(context) { }

        public List<ProductDTO> Get(Expression<Func<Product, bool>>? expression, int skip, int pageSize)
        {
            Context.Database.BeginTransaction();
            Context.Database.ExecuteSqlRaw("SET ANSI_WARNINGS OFF");

            if (expression != null)
                return DbSet.Include(p => p.ThumbImage)
                    .Where(expression)
                    .Skip(skip).Take(pageSize)
                    .Select(s_mapExpressionMin)
                    .OrderByDescending(p => p.TotalRating / p.RatingCount).ToList();

            return DbSet.Include(p => p.ThumbImage)
                .Skip(skip).Take(pageSize)
                .Select(s_mapExpressionMin)
                .OrderByDescending(p => p.TotalRating / p.RatingCount).ToList();
        }

        public int GetTotal(Expression<Func<Product, bool>>? expression)
        {
            if (expression != null)
                return DbSet.Count(expression);
            return DbSet.Count();
        }

        public PagedResult<ProductDTO> GetPaged(Expression<Func<Product, bool>>? expression, int pageIndex, int pageSize)
        {
            Context.Database.BeginTransaction();
            Context.Database.ExecuteSqlRaw("SET ANSI_WARNINGS OFF");

            IQueryable<Product> query = expression != null ?
                DbSet.Include(p => p.ThumbImage).Where(expression) :
                DbSet.Include(p => p.ThumbImage);

            int total = query.Count();
            List<ProductDTO> result = query
                .Skip(pageIndex * pageSize).Take(pageSize)
                .Select(s_mapExpressionMin)
                .OrderByDescending(p => p.TotalRating / p.RatingCount)
                .ToList();

            return new PagedResult<ProductDTO>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = total,
                Items = result
            };
        }

        public ProductFullDTO? GetById(string id, int commentTake, int reviewTake)
        {
            ProductFullDTO[] qResult = DbSet
                .Include(p => p.Shop)
                .Include(p => p.ShopCategory)
                .Include(p => p.Attributes)
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Where(p => p.Id == id)
                .Select(s_mapExpression)
                //.AsSplitQuery()
                .ToArray();

            if (qResult.Length == 0)
                return null;

            ProductFullDTO result = qResult[0];
            result.FirstComments = Context.Comments
                //...
                .Include(c => c.Customer)
                .Where(c => c.ProductId == id)
                .Take(commentTake).Select(CommentRepository.MapFuncWithCustomer);
            result.FirstReviews = Context.ProductReviews
                //...
                .Include(r => r.Customer)
                .Where(r => r.ProductId == id)
                .Take(reviewTake).Select(ProductReviewRepository.s_mapFunc);
            return result;
        }

        public Product? GetByIdMinimum(string id)
            => DbSet.Include(p => p.Images).FirstOrDefault(p => p.Id == id);

        public void LoadAttributes(Product product)
            => Context.Entry(product).Collection(p => p.Attributes).Load();

        public int GetPriceAfterDiscount(string id)
            => (int)DbSet.Where(p => p.Id == id).Take(1).Select(p => p.Price * (1 - p.Discount)).ToArray()[0];

        public void SafeDelete(Product product)
        {
            if (product.ThumbImageId != null)
                product.ThumbImage = null;
            Context.SaveChanges();
            DbSet.Remove(product);
        }
    }
}
