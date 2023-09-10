using System.ComponentModel.DataAnnotations.Schema;
using Commerce6.Data.Domain.AppUser;
using Commerce6.Data.Domain.Merchant;

namespace Commerce6.Data.Domain.Contact
{
    [Table("ProductReviews")]
    public class ProductReview
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

        [Column(TypeName = Datatype.VARCHAR45)]
        public string ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
