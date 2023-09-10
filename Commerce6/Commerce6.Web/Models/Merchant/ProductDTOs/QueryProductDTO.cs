namespace Commerce6.Web.Models.Merchant.ProductDTOs
{
    public class QueryProductDTO
    {
        //default order by rating

        //LIKE
        public string? Name { get; set; }

        //not including discount, either max or min
        public int? MaxPrice { get; set; }
        public int? MinPrice { get; set; }

        public DateTime? FromDate { get; set; }

        public int? ShopId { get; set; }

        public int? CategoryId { get; set; }

        public int Page { get; set; }                       //page = 0 -> first page
    }
}
