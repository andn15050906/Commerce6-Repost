using Commerce6.Data.Domain.Merchant;
using Commerce6.Web.Models.Merchant.CategoryDTOs;
using Commerce6.Web.Services.Abstraction;

namespace Commerce6.Web.Services.MerchantServices
{
    //admin only
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uow;

        public CategoryService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public StatusMessage Create(CreateCategoryDTO dto)
        {
            if (_uow.CategoryRepo.CategoryExisted(dto.Name))
                return StatusMessage.Conflict;

            string path = "";
            if (dto.Parent != null)
            {
                Category? parent = _uow.CategoryRepo.Find(dto.Parent);
                if (parent == null)
                    return StatusMessage.BadRequest;
                path = GeneratePath(parent.Path!, dto.Parent);
            }
            _uow.CategoryRepo.Insert(new Category { Path = path, Name = dto.Name, Description = dto.Description });
            _uow.Save();
            return StatusMessage.Created;
        }

        public StatusMessage Update(UpdateCategoryDTO dto)
        {
            Category? category = _uow.CategoryRepo.Find(dto.Id);
            if (category == null)
                return StatusMessage.BadRequest;
            if (dto.Name != null)
                category.Name = dto.Name;
            if (dto.Description != null)
                category.Description = dto.Description;
            _uow.Save();
            return StatusMessage.Ok;
        }

        public bool Delete(string name)
        {
            Category? category = _uow.CategoryRepo.FindByName(name);
            //Do NOT delete cascade
            if (category == null)
                return false;
            _uow.CategoryRepo.Delete(category);
            _uow.Save();
            return true;
        }

        private string GeneratePath(string parentPath, int? parentId) => $"{parentPath}-{parentId}-";
    }
}
