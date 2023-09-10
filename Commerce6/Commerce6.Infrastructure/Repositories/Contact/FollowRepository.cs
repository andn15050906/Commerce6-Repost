using Microsoft.EntityFrameworkCore;
using Commerce6.Data.Domain.Contact;
using Commerce6.Infrastructure.Models.AppUser;
using Commerce6.Infrastructure.Models.Merchant;

namespace Commerce6.Infrastructure.Repositories.Contact
{
    public sealed class FollowRepository : BaseRepository<Follow>
    {
        public FollowRepository(Context context) : base(context) { }

        public Follow? Get(string userId, int shopId)
            => DbSet.FirstOrDefault(_ => _.UserId == userId && _.ShopId == shopId);

        public List<UserMinDTO> GetFollowers(int shopId)
        {
            return DbSet.Include(f => f.User).Where(f => f.ShopId == shopId).Select(f => new UserMinDTO
            {
                Id = f.UserId,
                FullName = f.User.FullName,
                Avatar = f.User.Avatar
            }).ToList();
        }

        public List<ShopMinDTO> GetFollowings(string userId)
        {
            return DbSet.Include(f => f.Shop).Where(f => f.UserId == userId).Select(f => new ShopMinDTO
            {
                Id = f.ShopId,
                Name = f.Shop.Name,
                Avatar = f.Shop.Avatar
            }).ToList();
        }
    }
}
