using LibraryMS.BLL.DTOs.User;

namespace LibraryMS.BLL.Service.Abstraction
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}