using System.ComponentModel.DataAnnotations.Schema;

using Commerce6.Data.Domain.Contact;
using Commerce6.Data.Domain.Merchant;

namespace Commerce6.Data.Domain.AppUser
{
    [Table("Users")]
    public class User
    {
        [Column(TypeName = Datatype.VARCHAR45)]
        public string Id { get; set; }

        [Column(TypeName = Datatype.NVARCHAR45)]
        public string FullName { get; set; }

        [Column(TypeName = Datatype.VARCHAR15)]
        public string Phone { get; set; }

        [Column(TypeName = Datatype.VARCHAR100)]
        public string Password { get; set; }

        [Column(TypeName = Datatype.VARCHAR45)]
        public string Email { get; set; }

        [Column(TypeName = Datatype.DATE)]
        public DateTime DateOfBirth { get; set; }

        [Column(TypeName = Datatype.VARCHAR100)]
        public string? Avatar { get; set; }

        [Column(TypeName = Datatype.VARCHAR100)]
        public string? Facebook { get; set; }

        [Column(TypeName = Datatype.VARCHAR15)]
        public Role Role { get; set; }

        public DateTime JoinDate { get; set; }

        public DateTime LastSeen { get; set; }

        [Column(TypeName = Datatype.VARCHAR100)]
        public string? Token { get; set; }

        [Column(TypeName = Datatype.VARCHAR100)]
        public string? RefreshToken { get; set; }

        public bool IsConfirmed { get; set; }

        public int AccessFailedCount { get; set; }

        //OAuth
        [Column(TypeName = Datatype.VARCHAR100)]
        public string? LoginProvider { get; set; }
        [Column(TypeName = Datatype.VARCHAR100)]
        public string? ProviderKey { get; set; }

        public int? AddressId { get; set; }                     //prevent cascade
        public Address? Address { get; set; }

        public int? ShopId { get; set; }
        public Shop? Shop { get; set; }

        public ICollection<Shop>? Followed { get; set; }
    }
}