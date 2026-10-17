namespace LibraryMS.BLL.Service.Abstraction
{
    public interface ITokenService
    {
        (string Token, DateTime ExpiresAt) GenerateToken(ApplicationUser user, IList<string> roles);
    }
}
