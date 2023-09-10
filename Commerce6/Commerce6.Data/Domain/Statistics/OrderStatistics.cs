using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Commerce6.Data.Domain.Statistics
{
    [Table("OrderStatistics")]
    public class OrderStatistics
    {
        [Key]
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}
