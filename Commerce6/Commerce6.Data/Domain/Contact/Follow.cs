using System.ComponentModel.DataAnnotations.Schema;
using Commerce6.Data.Domain.AppUser;
using Commerce6.Data.Domain.Merchant;

namespace Commerce6.Data.Domain.Contact
{
    [Table("Follows")]
    public class Follow
    {
        [Column(TypeName = Datatype.VARCHAR45)]
        public string UserId { get; set; }
        public User? User { get; set; }

        public int ShopId { get; set; }
        public Shop? Shop { get; set; }
    }
}
