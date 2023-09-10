namespace Commerce6.Web.Services.Abstraction
{
    public interface IFollowService
    {
        StatusMessage Create(string? userId, int shopId);
        StatusMessage Delete(string? userId, int shopId);
    }
}
