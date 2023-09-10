namespace Commerce6.Web.Models.Merchant.ProductImageDTOs
{
    public class CreateProductImageDTO
    {
        //Only part of ProductDTOs
        public IFormFile Image { get; set; }
        public int Position { get; set; }           //0 = thumbImage
    }
}
