using System.ComponentModel.DataAnnotations.Schema;

namespace Commerce6.Data.Domain.Merchant
{
    [Table("ShopCategories")]
    public class ShopCategory
    {
        public int Id { get; set; }

        [Column(TypeName = Datatype.NVARCHAR45)]
        public string Name { get; set; }



        public int? ShopId { get; set; }
        public Shop? Shop { get; set; }

        //public ICollection<Product> Products { get; set; }
    }
}
