using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

using Commerce6.Data.Domain.AppUser;
using Commerce6.Infrastructure.Models.AppUser;
using Commerce6.Infrastructure.Models.Contact;

namespace Commerce6.Infrastructure.Repositories.AppUser
{
    public sealed class UserRepository : BaseRepository<User>
    {
        internal static readonly Expression<Func<User, UserDTO>> s_mapExpressionCustomer = _ => new UserDTO
        {
            Id = _.Id,
            FullName = _.FullName,
            Phone = _.Phone,
            Email = _.Email,
            DateOfBirth = _.DateOfBirth,
            Avatar = _.Avatar,
            Facebook = _.Facebook,
            JoinDate = _.JoinDate,
            LastSeen = _.LastSeen,
            Address = new AddressResponseDTO
            {
                Province = _.Address.Province,
                District = _.Address.District,
                Street = _.Address.Street,
                StreetNumber = _.Address.StreetNumber
            }
        };






        public UserRepository(Context context) : base(context) { }

        public bool EmailExisted(string email) => Any(u => u.Email == email);

        public void LoadAddress(User user) => Context.Entry(user).Reference(u => u.Address).Load();

        public User? FindByEmail(string email) => DbSet.FirstOrDefault(u => u.Email == email);

        public User? FindByPhoneOrEmail(string phoneOrEmail)
        {
            User? user = DbSet.FirstOrDefault(u => u.Email == phoneOrEmail);
            if (user == null)
                user = DbSet.FirstOrDefault(u => u.Phone == phoneOrEmail);
            return user;
        }

        public UserDTO? GetCustomer(string id)
        {
            UserDTO[] result = DbSet.Include(u => u.Address)
                .Where(u => u.Id == id)
                .Take(1)
                .Select(s_mapExpressionCustomer).ToArray();
            if (result.Length == 0)
                return null;
            return result[0];
        }

        public User? FindWithShop(string id) => DbSet.Include(u => u.Shop).FirstOrDefault(u => u.Id == id);

        public int? GetAddressId(string userId)
        {
            int?[] result = DbSet.Where(u => u.Id == userId).Take(1).Select(u => u.AddressId).ToArray();
            return result.Length > 0 ? result[0] : null;
        }
    }
}
