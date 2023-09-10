using Commerce6.Data.Domain.AppUser;
using Commerce6.Data.Domain.Contact;
using Commerce6.Data.Domain.Merchant;
using Commerce6.Infrastructure.Models.Merchant;
using Commerce6.Web.Services.Abstraction;
using Commerce6.Web.Models.Merchant.ShopDTOs;
using Commerce6.Web.Helpers.ServerFile;

namespace Commerce6.Web.Services.MerchantServices
{
    public class ShopService : IShopService
    {
        private readonly IUnitOfWork _uow;

        public ShopService(IUnitOfWork uow)
        {
            _uow = uow;
        }






        public ShopDTO? Get(int id)
        {
            return _uow.ShopRepo.Get(id);
        }

        public async Task<StatusMessage> Create(CreateShopDTO dto, string? userId)
        {
            if (userId == null)
                return StatusMessage.Unauthorized;
            User? user = _uow.UserRepo.Find(userId);
            if (user == null)
                return StatusMessage.Unauthorized;

            if (user.ShopId != null)
                return StatusMessage.Conflict;

            _uow.ShopRepo.Insert(await Adapt(dto, user));
            _uow.Save();
            return StatusMessage.Created;
        }

        public async Task<StatusMessage> Update(UpdateShopDTO dto, int? shopId)
        {
            if (shopId == null)
                return StatusMessage.Unauthorized;
            Shop? shop = _uow.ShopRepo.Find(shopId);
            if (shop == null)
                return StatusMessage.BadRequest;

            await ApplyChanges(dto, shop);
            _uow.Save();
            return StatusMessage.Ok;
        }

        public StatusMessage Delete(int? shopId)
        {
            if (shopId == null)
                return StatusMessage.Unauthorized;
            Shop? shop = _uow.ShopRepo.Find(shopId);
            if (shop == null)
                return StatusMessage.BadRequest;

            if (shop.AddressId != null)
            {
                _uow.ShopRepo.LoadAddress(shop);
                _uow.AddressRepo.Delete(shop.Address);
            }

            FileHelper fileHelper = new();
            fileHelper.DeleteFile(shop.Avatar, Dir.Shop);
            _uow.ShopRepo.Delete(shop);
            _uow.Save();
            return StatusMessage.Ok;
        }






        private async Task<Shop> Adapt(CreateShopDTO _, User user)
        {
            DateTime now = DateTime.Now;
            Shop shop = new()
            {
                Name = _.Name,
                Phone = _.Phone,
                CreatedAt = now,
                LastSeen = now,
                OwnerId = user.Id,
                Address = new Address
                {
                    Province = _.Address.Province,
                    District = _.Address.District,
                    Street = _.Address.Street,
                    StreetNumber = _.Address.StreetNumber
                }
            };
            if (_.Avatar != null)
            {
                FileHelper fileHelper = new();
                string? fileName = await fileHelper.SaveFile(_.Avatar, Dir.Shop);
                if (fileName != null)
                    shop.Avatar = fileName;
            }
            if (_.BankAccount != null)
                shop.BankAccount = _.BankAccount;

            user.Shop = shop;

            return shop;
        }
        
        private async Task ApplyChanges(UpdateShopDTO _, Shop shop)
        {
            if (_.Name != null)
                shop.Name = _.Name;
            if (_.Avatar != null)
            {
                FileHelper fileHelper = new();
                fileHelper.DeleteFile(shop.Avatar, Dir.Shop);
                string? fileName = await fileHelper.SaveFile(_.Avatar, Dir.Shop);
                if (fileName != null)
                    shop.Avatar = fileName;
            }
            if (_.Phone != null)
                shop.Phone = _.Phone;
            if (_.BankAccount != null)
                shop.BankAccount = _.BankAccount;
            if (_.Address != null)
            {
                _uow.ShopRepo.LoadAddress(shop);

                if (shop.Address != null)
                    _uow.AddressRepo.Delete(shop.Address);

                shop.Address = new Address
                {
                    Province = _.Address.Province,
                    District = _.Address.District,
                    Street = _.Address.Street,
                    StreetNumber = _.Address.StreetNumber
                };
            }
        }






        public StatusMessage CreateCategory(string name, int? shopId)
        {
            if (shopId == null)
                return StatusMessage.Unauthorized;

            if (String.IsNullOrEmpty(name) || name.Length > 45)
                return StatusMessage.BadRequest;
            if (_uow.ShopCategoryRepo.CategoryExisted(name, (int)shopId))
                return StatusMessage.Conflict;
            _uow.ShopCategoryRepo.Insert(new ShopCategory { Name = name, ShopId = shopId });
            _uow.Save();
            return StatusMessage.Created;
        }

        public StatusMessage UpdateCategory(int id, string name, int? shopId)
        {
            if (shopId == null)
                return StatusMessage.Unauthorized;

            if (String.IsNullOrEmpty(name) || name.Length > 45)
                return StatusMessage.BadRequest;
            if (_uow.ShopCategoryRepo.CategoryExisted(name, (int)shopId))
                return StatusMessage.Conflict;
            ShopCategory? category = _uow.ShopCategoryRepo.Find(id);
            if (category == null || category.ShopId != shopId)
                return StatusMessage.BadRequest;
            category.Name = name;
            _uow.Save();
            return StatusMessage.Ok;
        }

        public StatusMessage DeleteCategory(int id, int? shopId)
        {
            if (shopId == null)
                return StatusMessage.Unauthorized;

            ShopCategory? category = _uow.ShopCategoryRepo.Find(id);
            if (category == null || category.ShopId != shopId)
                return StatusMessage.BadRequest;
            _uow.ShopCategoryRepo.Delete(category);
            _uow.Save();
            return StatusMessage.Ok;
        }
    }
}
