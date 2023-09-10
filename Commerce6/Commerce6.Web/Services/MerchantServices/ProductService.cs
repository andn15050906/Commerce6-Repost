using System.Linq.Expressions;

using Commerce6.Data.Domain.Merchant;
using Commerce6.Infrastructure.Models.Merchant;
using Commerce6.Web.Services.Abstraction;
using Commerce6.Web.Models.Merchant.AttributeDTOs;
using Commerce6.Web.Models.Merchant.ProductDTOs;
using Commerce6.Web.Models.Merchant.ProductImageDTOs;
using Commerce6.Web.Helpers.ServerFile;
using Commerce6.Infrastructure.Models.Common;

namespace Commerce6.Web.Services.MerchantServices
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;

        private const int PAGE_SIZE = 2;

        public ProductService(IUnitOfWork uow)
        {
            _uow = uow;
        }






        // combine GetTotal for paging
        public List<ProductDTO> Get(QueryProductDTO query)
        {
            Expression<Func<Product, bool>>? predicate = GetPredicate(query);
            int skip = query.Page * PAGE_SIZE;
            return _uow.ProductRepo.Get(predicate, skip, PAGE_SIZE);
        }

        public PagedResult<ProductDTO> GetPaged(QueryProductDTO query)
        {
            Expression<Func<Product, bool>>? predicate = GetPredicate(query);
            return _uow.ProductRepo.GetPaged(predicate, query.Page, PAGE_SIZE);
        }

        public ProductFullDTO? GetById(string id, int commentTake, int reviewTake)
        {
            return _uow.ProductRepo.GetById(id, commentTake, reviewTake);
        }

        public async Task<StatusMessage> Create(CreateProductDTO dto, int? shopId)
        {
            if (shopId == null)
                return StatusMessage.Unauthorized;
            Shop? shop = _uow.ShopRepo.Find(shopId);
            if (shop == null)
                return StatusMessage.BadRequest;

            Category? category = _uow.CategoryRepo.Find(dto.CategoryId);
            if (category == null)
                return StatusMessage.BadRequest;

            (Product, ProductImage?) productAndThumb = await Adapt(dto, shop.Id);
            _uow.ProductRepo.Insert(productAndThumb.Item1);
            _uow.Save();
            if (productAndThumb.Item2 != null)
                productAndThumb.Item1.ThumbImageId = productAndThumb.Item2.Id;
            _uow.Save();
            
            return StatusMessage.Created;
        }

        public async Task<StatusMessage> Update(UpdateProductDTO dto, int? shopId)
        {
            if (shopId == null)
                return StatusMessage.Unauthorized;
            Shop? shop = _uow.ShopRepo.Find(shopId);
            if (shop == null)
                return StatusMessage.BadRequest;

            Product? product = _uow.ProductRepo.GetByIdMinimum(dto.Id);
            if (product == null)
                return StatusMessage.BadRequest;
            if (product.ShopId != shopId)
                return StatusMessage.Unauthorized;

            await ApplyChanges(dto, product);
            _uow.Save();
            return StatusMessage.Ok;
        }

        public StatusMessage Delete(string id, int? shopId)
        {
            if (shopId == null)
                return StatusMessage.Unauthorized;
            Product? product = _uow.ProductRepo.GetByIdMinimum(id);
            if (product == null)
                return StatusMessage.BadRequest;
            if (product.ShopId != shopId)
                return StatusMessage.Unauthorized;

            List<string> toDelete = new();
            foreach (ProductImage image in product.Images)
                toDelete.Add(image.Image);
            DeleteFiles(toDelete.ToArray(), new FileHelper());
            //has Cascade
            _uow.ProductRepo.SafeDelete(product);
            _uow.Save();
            return StatusMessage.Ok;
        }






        private Expression<Func<Product, bool>>? GetPredicate(QueryProductDTO query)
        {
            if (query.Name != null)
                return _ => _.Name.Contains(query.Name) && _.Stock > 0;
            if (query.MaxPrice != null)
                return _ => _.Price * (1 - _.Discount) <= query.MaxPrice && _.Stock > 0;
            if (query.MinPrice != null)
                return _ => _.Price * (1 - _.Discount) >= query.MinPrice && _.Stock > 0;
            if (query.FromDate != null)
                return _ => _.CreatedAt > query.FromDate && _.Stock > 0;
            if (query.ShopId != null)
                return _ => _.ShopId == query.ShopId && _.Stock > 0;
            if (query.CategoryId != null)
                return _ => _.Category.Id == query.CategoryId && _.Stock > 0;
            return null;
        }

        private async Task<(Product, ProductImage?)> Adapt(CreateProductDTO _, int shopId)
        {
            DateTime now = DateTime.Now;
            Product product = new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = _.Name,
                Price = _.Price,
                Description = _.Description,
                Discount = _.Discount,
                Stock = _.Stock,
                CreatedAt = now,
                UpdatedAt = now,
                ShopId = shopId,
                ShopCategoryId = _.ShopCategoryId,
                CategoryId = _.CategoryId
            };

            //Add attributes
            if (_.Attributes != null)
            {
                product.Attributes = new List<ProductAttribute>();
                foreach (AttributeRequestDTO attribute in _.Attributes)
                    product.Attributes.Add(new ProductAttribute { Name = attribute.Name, Value = attribute.Value });
            }

            //Add images
            List<ProductImage>? images = null;
            if (_.Images != null)
            {
                images = await SaveProductImages(_.Images, new FileHelper());
                if (images != null)
                {
                    foreach (ProductImage image in images)
                        image.Product = product;
                    product.Images = images;
                }
            }

            return (product, images == null ? null : images[0]);
        }

        private async Task ApplyChanges(UpdateProductDTO _, Product product)
        {
            if (_.Name != null)
                product.Name = _.Name;
            if (_.Price != null)
                product.Price = (int)_.Price;
            if (_.Description != null)
                product.Description = _.Description;
            if (_.Discount != null)
                product.Discount = (double)_.Discount;
            if (_.Stock != null)
                product.Stock = (int)_.Stock;

            //Update attributes
            if (_.DeletedAttributes != null || _.AddedAttributes != null)
                _uow.ProductRepo.LoadAttributes(product);
            if (_.DeletedAttributes != null)
                _uow.ProductAttributeRepo.RemoveRangeById(product.Id, _.DeletedAttributes);
            if (_.AddedAttributes != null)
            {
                foreach (AttributeRequestDTO attribute in _.AddedAttributes)
                    product.Attributes.Add(new ProductAttribute { Name = attribute.Name, Value = attribute.Value });
            }

            //Update images
            FileHelper fileHelper = new();
            if (_.DeletedImages != null)
            {
                List<string> toDelete = new();
                foreach (string fileName in _.DeletedImages)
                {
                    ProductImage? image = product.Images.FirstOrDefault(i => i.Image == fileName);
                    if (image != null)
                    {
                        product.Images.Remove(image);
                        toDelete.Add(fileName);
                    }
                }
                DeleteFiles(toDelete.ToArray(), fileHelper);
            }
            if (_.AddedImages != null)
            {
                //calculate position based on existing images
                int index = 0;
                ProductImage? lastImg = product.Images.LastOrDefault();
                if (lastImg != null)
                    index += lastImg.Position + 1;

                List<ProductImage>? images = await SaveProductImages(_.AddedImages, fileHelper);
                foreach (ProductImage image in images)
                {
                    image.Position += index;
                    product.Images.Add(image);
                }
                //..
                product.ThumbImage = product.Images.First();
            }

            product.UpdatedAt = DateTime.Now;
        }






        private async Task<List<ProductImage>?> SaveProductImages(CreateProductImageDTO[] dtos, FileHelper helper)
        {
            IFormFile[] files = new IFormFile[dtos.Length];
            int[] position = new int[dtos.Length];
            int i;
            for (i = 0; i < dtos.Length; i++)
            {
                files[i] = dtos[i].Image;
                position[i] = dtos[i].Position;
            }
            string[]? fileNames = await helper.SaveFiles(files, Dir.Product);

            if (fileNames == null)
                return null;
            List<ProductImage> images = new();
            for (i = 0; i < position.Length; i++)
                images.Add(new ProductImage { Image = fileNames[i], Position = position[i] });
            return images;
        }
        
        private async Task DeleteFiles(string[] fileNames, FileHelper helper)
        {
            await helper.DeleteFiles(fileNames, Dir.Product);
        }
    }
}
