using Commerce6.Data.Domain.AppUser;
using Commerce6.Data.Domain.Contact;
using Commerce6.Data.Domain.Merchant;
using Commerce6.Infrastructure.Models.AppUser;
using Commerce6.Infrastructure.Models.Contact;
using Commerce6.Web.Services.Abstraction;
using Commerce6.Web.Models.Merchant.ShopReviewDTOs;

namespace Commerce6.Web.Services.ContactServices
{
    public class ShopReviewService : IShopReviewService
    {
        private readonly IUnitOfWork _uow;

        public ShopReviewService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public List<ShopReviewDTO> Get(int shopId)
        {
            return _uow.ShopReviewRepo.Get(shopId);
        }

        public (ShopReviewDTO?, StatusMessage) Create(CreateShopReviewDTO dto, string? userId)
        {
            if (userId == null)
                return (null, StatusMessage.Unauthorized);
            User? user = _uow.UserRepo.Find(userId);
            if (user == null)
                return (null, StatusMessage.Unauthorized);

            Shop? shop = _uow.ShopRepo.Find(dto.ShopId);
            if (shop == null)
                return (null, StatusMessage.BadRequest);

            ShopReview review = Adapt(dto, user);
            _uow.ShopReviewRepo.Insert(review);
            _uow.Save();
            return (Map(review), StatusMessage.Created);
        }

        public (ShopReviewDTO?, StatusMessage) Update(UpdateShopReviewDTO dto, string? userId)
        {
            if (userId == null)
                return (null, StatusMessage.Unauthorized);
            ShopReview? review = _uow.ShopReviewRepo.GetById(dto.Id);
            if (review == null)
                return (null, StatusMessage.BadRequest);
            if (review.CustomerId != userId)
                return (null, StatusMessage.Unauthorized);

            ApplyChanges(dto, review);
            _uow.Save();
            return (Map(review), StatusMessage.Ok);
        }

        public StatusMessage Delete(int id, string? userId)
        {
            if (userId == null)
                return StatusMessage.Unauthorized;
            ShopReview? review = _uow.ShopReviewRepo.Find(id);
            if (review == null)
                return StatusMessage.BadRequest;
            if (review.CustomerId != userId)
                return StatusMessage.Unauthorized;
            _uow.ShopReviewRepo.Delete(review);
            _uow.Save();
            return StatusMessage.Ok;
        }






        private ShopReviewDTO Map(ShopReview _)
        {
            return new ShopReviewDTO
            {
                Id = _.Id,
                Content = _.Content,
                Rating = _.Rating,
                CreatedAt = _.CreatedAt,
                ModifiedAt = _.ModifiedAt,
                Customer = new UserMinDTO
                {
                    Id = _.CustomerId,
                    FullName = _.Customer.FullName,
                    Avatar = _.Customer.Avatar
                },
                ShopId = _.ShopId
            };
        }

        private ShopReview Adapt(CreateShopReviewDTO _, User user)
        {
            DateTime now = DateTime.Now;
            return new()
            {
                Content = _.Content,
                Rating = _.Rating,
                CreatedAt = now,
                ModifiedAt = now,
                Customer = user,
                ShopId = _.ShopId
            };
        }

        private void ApplyChanges(UpdateShopReviewDTO _, ShopReview review)
        {
            review.Content = _.Content;
            review.Rating = _.Rating;
            review.ModifiedAt = DateTime.Now;
        }
    }
}
