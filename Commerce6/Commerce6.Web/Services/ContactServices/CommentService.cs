using System.Linq.Expressions;
using System.Text.RegularExpressions;

using Commerce6.Data.Domain.AppUser;
using Commerce6.Data.Domain.Contact;
using Commerce6.Data.Domain.Merchant;
using Commerce6.Infrastructure.Models.AppUser;
using Commerce6.Infrastructure.Models.Contact;
using Commerce6.Infrastructure.Models.Merchant;
using Commerce6.Web.Helpers.ServerFile;
using Commerce6.Web.Models.Contact.CommentDTOs;
using Commerce6.Web.Services.Abstraction;

namespace Commerce6.Web.Services.ContactServices
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _uow;

        //Client remove "*{FileString}"
        private const char CONTENT_SEPARATOR = '*';
        private const char SEPARATOR = ':';

        public CommentService(IUnitOfWork uow)
        {
            _uow = uow;
        }






        public List<CommentDTO> Get(QueryCommentDTO query)
        {
            return _uow.CommentRepo.Get(GetPredicate(query));
        }

        /*public CommentDTO? GetById(int id)
        {
            return _uow.CommentRepo.GetById(id);
        }*/

        public async Task<(CommentDTO?, StatusMessage)> Create(CreateCommentDTO dto, string? userId = null, int? shopId = null)
        {
            Product? product = _uow.ProductRepo.GetByIdMinimum(dto.ProductId);
            if (product == null)
                return (null, StatusMessage.BadRequest);
            bool isShopComment = false;
            User? user = null;

            //check if the comment is from a customer or the merchant
            if (shopId != null && product.ShopId == shopId)
                isShopComment = true;
            else
            {
                if (userId == null)
                    return (null, StatusMessage.Unauthorized);
                user = _uow.UserRepo.Find(userId);
                if (user == null)
                    return (null, StatusMessage.Unauthorized);
            }

            string path = "";
            if (dto.Parent != null)
            {
                Comment? parent = _uow.CommentRepo.Find(dto.Parent);
                if (parent == null || parent.ProductId != dto.ProductId)
                    return (null, StatusMessage.BadRequest);
                path = GeneratePath(parent.Path!, dto.Parent);
            }

            Comment comment = isShopComment ?
                await Adapt(dto, path, product, null, shopId) :
                await Adapt(dto, path, product, user, null);
            _uow.CommentRepo.Insert(comment);
            _uow.Save();
            return (Map(comment), StatusMessage.Created);
        }

        public async Task<(CommentDTO?, StatusMessage)> Update(UpdateCommentDTO dto, string? userId = null, int? shopId = null)
        {
            if (userId == null && shopId == null)
                return (null, StatusMessage.Unauthorized);
            Comment? comment = _uow.CommentRepo.Find(dto.Id);
            if (comment == null)
                return (null, StatusMessage.BadRequest);


            bool isNotShopComment = shopId == null || comment.ShopId != shopId;
            if (isNotShopComment && comment.CustomerId != userId)
                return (null, StatusMessage.Unauthorized);

            if (isNotShopComment)
                _uow.CommentRepo.LoadCustomer(comment);
            else
                _uow.CommentRepo.LoadShop(comment);
            await ApplyChanges(dto, comment);
            _uow.Save();
            return (Map(comment), StatusMessage.Ok);
        }

        public StatusMessage Delete(int id, string? userId = null, int? shopId = null)
        {
            if (userId == null && shopId == null)
                return StatusMessage.Unauthorized;
            Comment? comment = _uow.CommentRepo.Find(id);
            if (comment == null)
                return StatusMessage.BadRequest;
            if (comment.ShopId != shopId && comment.CustomerId != userId)
                return StatusMessage.Unauthorized;

            DeleteFiles(GetFiles(GetFileString(comment.Content)), new FileHelper());
            _uow.CommentRepo.Delete(comment);
            _uow.Save();
            return StatusMessage.Ok;
        }






        private CommentDTO Map(Comment _)
        {
            CommentDTO dto = new()
            {
                Id = _.Id,
                Content = _.Content,
                Path = _.Path,
                CreatedAt = _.CreatedAt,
                ModifiedAt = _.ModifiedAt,
                Hidden = _.Hidden,
                ProductId = _.ProductId
            };
            if (_.Customer != null)
                dto.Customer = new UserMinDTO { Id = _.Customer!.Id, FullName = _.Customer.FullName, Avatar = _.Customer.Avatar };
            else
                dto.Shop = new ShopMinDTO { Id = _.Shop!.Id, Name = _.Shop.Name, Avatar = _.Shop.Avatar };
            return dto;
        }

        private Expression<Func<Comment, bool>>? GetPredicate(QueryCommentDTO query)
        {
            if (query.ParentId != null)
                return _ => _.Path.Contains("-" + query.ParentId + "-");
            if (query.ProductId != null)
                return _ => _.ProductId == query.ProductId;
            if (query.FromDate != null)
                return _ => _.CreatedAt > query.FromDate;
            if (query.CustomerId != null)
                return _ => _.Customer.Id == query.CustomerId;
            if (query.ShopId != null)
                return _ => _.Shop.Id == query.ShopId;
            return null;
        }

        private async Task<Comment> Adapt(CreateCommentDTO _, string path, Product product, User? user, int? shopId = null)
        {
            DateTime now = DateTime.Now;
            Comment comment = new()
            {
                Content = _.Content,
                Path = path,
                CreatedAt = now,
                ModifiedAt = now,
                Product = product
            };
            if (shopId != null)
                comment.Shop = _uow.ShopRepo.Find(shopId);
            else
                comment.Customer = user;
            //add files to the end
            if (_.Files != null)
            {
                System.Diagnostics.Debug.WriteLine("File found.");
                string? fileString = await SaveAndGetFileString(_.Files, new FileHelper());
                comment.Content += CONTENT_SEPARATOR + fileString;
            }

            return comment;
        }

        private async Task ApplyChanges(UpdateCommentDTO _, Comment comment)
        {
            comment.ModifiedAt = DateTime.Now;
            if (_.Hidden == true)
                comment.Hidden = true;

            FileHelper helper = new();
            string? fileString = GetFileString(comment.Content);
            if (fileString != null && _.DeletedFiles != null)
            {
                foreach (string file in _.DeletedFiles)
                    fileString = fileString.Replace(file, "");
                DeleteFiles(_.DeletedFiles, helper);
            }
            fileString += await SaveAndGetFileString(_.AddedFiles, helper);
            fileString = Regex.Replace(fileString.Trim(':'), ":+", ":");            //format the fileString

            string mainContent = _.Content ?? GetMainContent(comment.Content);
            comment.Content = mainContent + '*' + fileString;
        }






        private string GeneratePath(string parentPath, int? parentId) => $"{parentPath}-{parentId}-";

        private async Task<string> SaveAndGetFileString(IFormFile[]? files, FileHelper helper)
        {
            string[]? result = await helper.SaveFiles(files, Dir.Comment);

            //convert filenames into content
            if (result == null)
                return "";
            string concat = "";
            int i;
            for (i = 0; i < files.Length - 1; i++)
                concat += result[i] + SEPARATOR;
            concat += result[files.Length - 1];
            return concat;
        }

        private async Task DeleteFiles(string[] fileNames, FileHelper helper)
        {
            await helper.DeleteFiles(fileNames, Dir.Comment);
        }

        private string? GetFileString(string content)
        {
            int index = content.LastIndexOf(CONTENT_SEPARATOR);
            if (index != -1)
                return content[(index + 1)..];
            return null;
        }

        private string GetMainContent(string content)
        {
            int index = content.LastIndexOf(CONTENT_SEPARATOR);
            if (index != -1)
                return content[..index];
            return content;
        }

        private string[]? GetFiles(string fileString)
        {
            return fileString.Split(SEPARATOR);
        }
    }
}
