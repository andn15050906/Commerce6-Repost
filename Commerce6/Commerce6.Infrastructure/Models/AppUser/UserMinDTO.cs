namespace Commerce6.Infrastructure.Models.AppUser
{
    public class UserMinDTO
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Avatar { get; set; }
    }
}
