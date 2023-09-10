using System.ComponentModel.DataAnnotations.Schema;

using Commerce6.Data.Domain.AppUser;
using Commerce6.Data.Domain.Contact;
using Commerce6.Data.Domain.Sale;

namespace Commerce6.Data.Domain.Merchant
{
    [Table("Shops")]
    public class Shop
    {
        public int Id { get; set; }

        [Column(TypeName = Datatype.NVARCHAR100)]
        public string Name { get; set; }

        [Column(TypeName = Datatype.VARCHAR100)]
        public string? Avatar { get; set; }

        [Column(TypeName = Datatype.VARCHAR15)]
        public string Phone { get; set; }

        [Column(TypeName = Datatype.VARCHAR20)]
        public string? BankAccount { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastSeen { get; set; }



        //Need triggers, Calc AverageRating using RatingCount & TotalRating

        public int ProductCount { get; set; }

        public int FollowerCount { get; set; }

        public int RatingCount { get; set; }

        public int TotalRating { get; set; }



        [Column(TypeName = Datatype.VARCHAR45)]
        public string OwnerId { get; set; }
        public User? Owner { get; set; }

        public int? AddressId { get; set; }                             //prevent cascade
        public Address? Address { get; set; }

        public virtual ICollection<User>? Followers { get; set; }
        //public ICollection<Order>? Orders { get; set; }
        //public ICollection<Promotion>? Promotion { get; set; }
        //public ICollection<ShopCategory>? ShopCategories { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
        public virtual ICollection<ShopReview>? Reviews { get; set; }
    }
}
