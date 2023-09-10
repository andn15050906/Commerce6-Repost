using System.ComponentModel.DataAnnotations.Schema;

namespace Commerce6.Data.Domain.Merchant
{
    [Table("ProductAttributes")]
    public class ProductAttribute
    {
        public int Id { get; set; }

        [Column(TypeName = Datatype.NVARCHAR45)]
        public string Name { get; set; }

        [Column(TypeName = Datatype.NVARCHAR45)]
        public string Value { get; set; }

        [Column(TypeName = Datatype.VARCHAR45)]
        public string ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
