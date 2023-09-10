using System.ComponentModel.DataAnnotations.Schema;

namespace Commerce6.Data.Domain.Contact
{
    [Table("Addresses")]
    public class Address
    {
        public int Id { get; set; }

        [Column(TypeName = Datatype.NVARCHAR45)]
        public string Province { get; set; }

        [Column(TypeName = Datatype.NVARCHAR45)]
        public string District { get; set; }

        [Column(TypeName = Datatype.NVARCHAR45)]
        public string Street { get; set; }

        [Column(TypeName = Datatype.NVARCHAR45)]
        public string StreetNumber { get; set; }
    }
}
