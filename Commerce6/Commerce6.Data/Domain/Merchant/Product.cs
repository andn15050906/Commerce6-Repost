using System.ComponentModel.DataAnnotations.Schema;
using Commerce6.Data.Domain.Contact;

namespace Commerce6.Data.Domain.Merchant
{
    [Table("Products")]
    public class Product
    {
        [Column(TypeName = Datatype.VARCHAR45)]
        public string Id { get; set; }

        [Column(TypeName = Datatype.NVARCHAR100)]
        public string Name { get; set; }

        public int Price { get; set; }

        [Column(TypeName = Datatype.NVARCHAR1000)]
        public string? Description { get; set; }

        public double Discount { get; set; }

        public int Stock { get; set; }                  //need trigger_1

        public int Sold { get; set; }                   //need trigger_1

        [Column(TypeName = Datatype.NVARCHAR45)]
        public string? CountUnit { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [Column(TypeName = Datatype.NVARCHAR45)]
        public string? ServerTag { get; set; }          //top sold ...

        public int ViewCount { get; set; }



        //Need triggers, Calc AverageRating using RatingCount & TotalRating

        public int RatingCount { get; set; }

        public int TotalRating { get; set; }



        public int ShopId { get; set; }
        public Shop? Shop { get; set; }

        public int? ShopCategoryId { get; set; }
        public ShopCategory? ShopCategory { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int? ThumbImageId { get; set; }
        //not loaded in ProductDTO
        public ProductImage? ThumbImage { get; set; }

        public ICollection<ProductAttribute>? Attributes { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<ProductReview>? Reviews { get; set; }
        public ICollection<ProductImage>? Images { get; set; }
    }
}
