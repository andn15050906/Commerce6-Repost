namespace Commerce6.Web.Models.Contact.CommentDTOs
{
    public class UpdateCommentDTO
    {
        public int Id { get; set; }
        public bool? Hidden { get; set; }                       //only on change

        public string Content { get; set; }                     //always send this
        public string[]? DeletedFiles { get; set; }             //only on deleting, send fileName, not id
        public IFormFile[]? AddedFiles { get; set; }            //only on adding
    }
}
