namespace Commerce6.Web.Models.Contact.CommentDTOs
{
    public class CreateCommentDTO
    {
        public string ProductId { get; set; } = null!;
        public int? Parent { get; set; }
        public string Content { get; set; } = null!;
        public IFormFile[]? Files { get; set; }
    }
}
