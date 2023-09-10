namespace Commerce6.Web.Models.Contact.CommentDTOs
{
    public class QueryCommentDTO
    {
        public int? ParentId { get; set; }
        public string? ProductId { get; set; }
        public string? CustomerId { get; set; }         //if author is customer
        public int? ShopId { get; set; }                //if author is shop
        public DateTime? FromDate { get; set; }
    }
}
