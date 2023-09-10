using Commerce6.Data.Domain.AppUser;
using Commerce6.Data.Domain.Contact;
using Commerce6.Data.Domain.Merchant;
using Commerce6.Infrastructure.Models.AppUser;
using Commerce6.Infrastructure.Models.Contact;
using Commerce6.Web.Services.Abstraction;
using Commerce6.Web.Models.Merchant.ProductReviewDTOs;

namespace Commerce6.Web.Services.ContactServices
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly IUnitOfWork _uow;

        public ProductReviewService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public List<ProductReviewDTO> Get(string productId)
        {
            return _uow.ProductReviewRepo.Get(productId);
        }

        public (ProductReviewDTO?, StatusMessage) Create(CreateProductReviewDTO dto, string? userId)
        {
            if (userId == null)
                return (null, StatusMessage.Unauthorized);
            User? user = _uow.UserRepo.Find(userId);
            if (user == null)
                return (null, StatusMessage.Unauthorized);

            Product? product = _uow.ProductRepo.Find(dto.ProductId);
            if (product == null)
                return (null, StatusMessage.BadRequest);

            ProductReview review = Adapt(dto, user);
            _uow.ProductReviewRepo.Insert(review);
            _uow.Save();
            return (Map(review), StatusMessage.Created);
        }

        public (ProductReviewDTO?, StatusMessage) Update(UpdateProductReviewDTO dto, string? userId)
        {
            if (userId == null)
                return (null, StatusMessage.Unauthorized);
            ProductReview? review = _uow.ProductReviewRepo.GetById(dto.Id);
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
            ProductReview? review = _uow.ProductReviewRepo.Find(id);
            if (review == null)
                return StatusMessage.BadRequest;
            if (review.CustomerId != userId)
                return StatusMessage.Unauthorized;
            _uow.ProductReviewRepo.Delete(review);
            _uow.Save();
            return StatusMessage.Ok;
        }






        private ProductReviewDTO Map(ProductReview _)
        {
            return new ProductReviewDTO
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
                ProductId = _.ProductId
            };
        }

        private ProductReview Adapt(CreateProductReviewDTO _, User user)
        {
            DateTime now = DateTime.Now;
            return new()
            {
                Content = _.Content,
                Rating = _.Rating,
                CreatedAt = now,
                ModifiedAt = now,
                Customer = user,
                ProductId = _.ProductId
            };
        }

        private void ApplyChanges(UpdateProductReviewDTO _, ProductReview review)
        {
            review.Content = _.Content;
            review.Rating = _.Rating;
            review.ModifiedAt = DateTime.Now;
        }
    }
}
