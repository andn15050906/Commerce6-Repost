using Commerce6.Data.Domain.Contact;
using Commerce6.Web.Services.Abstraction;

namespace Commerce6.Web.Services.ContactServices
{
    public class FollowService : IFollowService
    {
        private readonly IUnitOfWork _uow;

        public FollowService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public StatusMessage Create(string? userId, int shopId)
        {
            if (userId == null)
                return StatusMessage.Unauthorized;
            try
            {
                _uow.FollowRepo.Insert(new Follow { UserId = userId, ShopId = shopId });
                _uow.Save();
                return StatusMessage.Created;
            }
            catch (Exception)
            {
                return StatusMessage.BadRequest;
            }
        }

        public StatusMessage Delete(string? userId, int shopId)
        {
            if (userId == null)
                return StatusMessage.Unauthorized;
            Follow? follow = _uow.FollowRepo.Get(userId, shopId);
            if (follow == null)
                return StatusMessage.BadRequest;

            _uow.FollowRepo.Delete(follow);
            _uow.Save();
            return StatusMessage.Ok;
        }
    }
}
