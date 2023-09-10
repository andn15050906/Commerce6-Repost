using System.ComponentModel.DataAnnotations.Schema;

namespace Commerce6.Data.Domain.Merchant
{
    [Table("ProductImages")]
    public class ProductImage
    {
        public int Id { get; set; }

        [Column(TypeName = Datatype.VARCHAR100)]
        public string Image { get; set; }

        public int Position { get; set; }       //or Order



        [Column(TypeName = Datatype.VARCHAR45)]
        public string ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
