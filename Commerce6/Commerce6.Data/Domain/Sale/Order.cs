using System.ComponentModel.DataAnnotations.Schema;
using Commerce6.Data.Domain.AppUser;
using Commerce6.Data.Domain.Contact;
using Commerce6.Data.Domain.Merchant;

namespace Commerce6.Data.Domain.Sale
{
    [Table("Orders")]
    public class Order
    {
        [Column(TypeName = Datatype.VARCHAR45)]
        public string Id { get; set; }

        [Column(TypeName = Datatype.VARCHAR45)]
        public string PaymentMethod { get; set; }

        [Column(TypeName = Datatype.VARCHAR45)]
        public string Transporter { get; set; }

        [Column(TypeName = Datatype.VARCHAR15)]
        public OrderState State { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime? CompletedTime { get; set; }

        public int Fee { get; set; }

        public int Price { get; set; }

        public double Discount { get; set; }



        [Column(TypeName = Datatype.VARCHAR45)]
        public string? CustomerId { get; set; }
        public User? Customer { get; set; }

        public int? FromAddressId { get; set; }                     //prevent cascade
        public Address? FromAddress { get; set; }

        public int? ToAddressId { get; set; }                       //prevent cascade
        public Address? ToAddress { get; set; }

        public int? ShopId { get; set; }
        public Shop? Shop { get; set; }

        //
        public virtual ICollection<Order_Product>? Order_Products { get; set; }
    }
}
