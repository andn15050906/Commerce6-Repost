namespace Commerce6.Web.Models.Merchant.CategoryDTOs
{
    public class UpdateCategoryDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
