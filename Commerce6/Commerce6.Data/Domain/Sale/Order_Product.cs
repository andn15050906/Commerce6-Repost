using System.ComponentModel.DataAnnotations.Schema;
using Commerce6.Data.Domain.Merchant;

namespace Commerce6.Data.Domain.Sale
{
    [Table("Order_Products")]
    public class Order_Product
    {
        public int Quantity { get; set; }

        [Column(TypeName = Datatype.VARCHAR45)]
        public string OrderId { get; set; }
        public Order? Order { get; set; }

        [Column(TypeName = Datatype.VARCHAR45)]
        public string ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
