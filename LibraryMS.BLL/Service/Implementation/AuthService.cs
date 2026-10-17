using LibraryMS.BLL.DTOs.User;
using Microsoft.AspNetCore.Identity;

namespace LibraryMS.BLL.Service.Implementation
{
    public class AuthService : IAuthService
    {
        private static readonly string[] AllowedRoles = { "Admin", "Librarian" };

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            if (!AllowedRoles.Contains(dto.Role))
                throw new BusinessRuleException($"Role must be one of: {string.Join(", ", AllowedRoles)}.");

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser is not null)
                throw new BusinessRuleException($"A user with email {dto.Email} already exists.");

            var user = ApplicationUser.CreateUser(dto.FullName, dto.Email);

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BusinessRuleException($"Registration failed: {errors}");
            }

            if (!await _roleManager.RoleExistsAsync(dto.Role))
                throw new NotFoundException($"Role '{dto.Role}' not found.");

            await _userManager.AddToRoleAsync(user, dto.Role);

            var (token, expiresAt) = _tokenService.GenerateToken(user, new List<string> { dto.Role });

            return new AuthResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                Role = dto.Role,
                Token = token,
                ExpiresAt = expiresAt
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                throw new BusinessRuleException("Invalid email or password.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
                throw new BusinessRuleException("Invalid email or password.");

            var roles = await _userManager.GetRolesAsync(user);

            var (token, expiresAt) = _tokenService.GenerateToken(user, roles);

            return new AuthResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                Role = roles.FirstOrDefault() ?? string.Empty,
                Token = token,
                ExpiresAt = expiresAt
            };
        }
    }
}