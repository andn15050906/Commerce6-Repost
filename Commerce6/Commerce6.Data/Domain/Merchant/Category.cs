using System.ComponentModel.DataAnnotations.Schema;

namespace Commerce6.Data.Domain.Merchant
{
    [Table("Categories")]
    public class Category
    {
        public int Id { get; set; }

        [Column(TypeName = Datatype.VARCHAR45)]
        public string? Path { get; set; }

        [Column(TypeName = Datatype.NVARCHAR45)]
        public string Name { get; set; }

        [Column(TypeName = Datatype.NVARCHAR255)]
        public string? Description { get; set; }



        //public ICollection<Product>? Products { get; set; }
    }
}
