using System.ComponentModel.DataAnnotations.Schema;
using Commerce6.Data.Domain.AppUser;
using Commerce6.Data.Domain.Merchant;

namespace Commerce6.Data.Domain.Contact
{
    [Table("ShopReviews")]
    public class ShopReview
    {
        public int Id { get; set; }

        [Column(TypeName = Datatype.NVARCHAR255)]
        public string? Content { get; set; }

        public int Rating { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }



        [Column(TypeName = Datatype.VARCHAR45)]
        public string CustomerId { get; set; }
        public User? Customer { get; set; }

        public int ShopId { get; set; }
        public Shop? Shop { get; set; }
    }
}
