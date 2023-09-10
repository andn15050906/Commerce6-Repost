using System.ComponentModel.DataAnnotations.Schema;

namespace Commerce6.Data.Domain.Merchant
{
    [Table("Promotions")]
    public class Promotion
    {
        public int Id { get; set; }

        [Column(TypeName = Datatype.VARCHAR100)]
        public string Image { get; set; }

        [Column(TypeName = Datatype.VARCHAR100)]
        public string Url { get; set; }



        public int ShopId { get; set; }
        public Shop? Shop { get; set; }
    }
}
