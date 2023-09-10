using Commerce6.Infrastructure.Models.AppUser;
using Commerce6.Infrastructure.Models.Merchant;

namespace Commerce6.Infrastructure.Models.Contact
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public string? Path { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool Hidden { get; set; }
        public string ProductId { get; set; } = null!;

        public UserMinDTO? Customer { get; set; }               //if author is customer
        public ShopMinDTO? Shop { get; set; }                   //if author is shop
    }
}
