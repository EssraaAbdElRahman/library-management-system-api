using System.ComponentModel.DataAnnotations;

namespace LibraryMS.BLL.DTOs.User
{
    public class RegisterDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Role { get; set; } = "Librarian"; // Admin أو Librarian
    }
}
