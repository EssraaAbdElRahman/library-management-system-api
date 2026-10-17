
using Microsoft.AspNetCore.Identity;

namespace LibraryMS.DAL.Entities
{
    public class ApplicationUser : IdentityUser

    {
        public string FullName { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public ApplicationUser()
        {
        }

        public static ApplicationUser CreateUser(string fullName, string email)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full name is required");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required");

            return new ApplicationUser
            {
                FullName = fullName.Trim(),
                UserName = email.Trim(),
                Email = email.Trim(),
                CreatedAt = DateTime.Now
            };
        }

        public void UpdateProfile(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full name is required");

            FullName = fullName.Trim();
            UpdatedAt = DateTime.Now;
        }
    }
}