namespace Commerce6.Web.Models.Merchant.CategoryDTOs
{
    public class CreateCategoryDTO
    {
        public string Name { get; set; } = null!;
        public int? Parent { get; set; }
        public string? Description { get; set; }
    }
}
