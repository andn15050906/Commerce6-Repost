using Commerce6.Infrastructure.Models.Contact;
using Commerce6.Web.Models.Contact.CommentDTOs;

namespace Commerce6.Web.Services.Abstraction
{
    public interface ICommentService
    {
        List<CommentDTO> Get(QueryCommentDTO query);
        Task<(CommentDTO?, StatusMessage)> Create(CreateCommentDTO dto, string? userId, int? shopId);
        Task<(CommentDTO?, StatusMessage)> Update(UpdateCommentDTO dto, string? userId, int? shopId);
        StatusMessage Delete(int id, string? userId, int? shopId);
    }
}
