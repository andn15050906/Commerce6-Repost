using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

using Commerce6.Data.Domain.Contact;
using Commerce6.Infrastructure.Models.Contact;
using Commerce6.Infrastructure.Models.AppUser;
using Commerce6.Infrastructure.Models.Merchant;

namespace Commerce6.Infrastructure.Repositories.Contact
{
    public sealed class CommentRepository : BaseRepository<Comment>
    {
        //requires loaded Customer
        internal static readonly Func<Comment, CommentDTO> MapFuncWithCustomer = _ => new CommentDTO()
        {
            Id = _.Id,
            Content = _.Content,
            Path = _.Path,
            CreatedAt = _.CreatedAt,
            ModifiedAt = _.ModifiedAt,
            Hidden = _.Hidden,
            ProductId = _.ProductId,
            Customer = new UserMinDTO
            {
                Id = _.Customer.Id,
                FullName = _.Customer.FullName,
                Avatar = _.Customer.Avatar
            }
        };






        public CommentRepository(Context context) : base(context) { }

        public List<CommentDTO> Get(Expression<Func<Comment, bool>>? expression)
        {
            List<Comment> qResult;
            List<CommentDTO> result = new();

            if (expression != null)
                qResult = DbSet.Include(c => c.Customer).Include(c => c.Shop)
                    .Where(expression)
                    .OrderByDescending(c => c.ModifiedAt).ToList();
            else
                qResult = DbSet.Include(c => c.Customer).Include(c => c.Shop)
                    .OrderByDescending(c => c.ModifiedAt).ToList();

            foreach (Comment _ in qResult)
            {
                CommentDTO dto = new();

                dto.Id = _.Id;
                dto.Content = _.Content;
                dto.Path = _.Path;
                dto.CreatedAt = _.CreatedAt;
                dto.ModifiedAt = _.ModifiedAt;
                dto.Hidden = _.Hidden;
                dto.ProductId = _.ProductId;
                if (_.CustomerId != null)
                    dto.Customer = new UserMinDTO { Id = _.CustomerId, FullName = _.Customer.FullName, Avatar = _.Customer.Avatar };
                else
                    dto.Shop = new ShopMinDTO { Id = _.Shop.Id, Name = _.Shop.Name, Avatar = _.Shop.Avatar };

                result.Add(dto);
            }

            return result;
        }

        public void LoadCustomer(Comment comment)
            => Context.Entry(comment).Reference(c => c.Customer).Load();

        public void LoadShop(Comment comment)
            => Context.Entry(comment).Reference(c => c.Shop).Load();

        /*public CommentDTO? GetById(int id)
        {
            CommentDTO[] result = DbSet.Include(c => c.Customer).Include(c => c.Shop)
                .Where(p => p.Id == id)
                .Take(1)
                .Select(MapExpression).ToArray();
            if (result.Length == 0)
                return null;
            return result[0];
        }*/
    }
}
