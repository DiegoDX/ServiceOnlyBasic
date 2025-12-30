using Core.Common;
using Core.Exceptions;
using static Core.Utils.Enums;

namespace Core.Entities
{
    /// <summary>
    /// Represents a User in the system.
    /// </summary>
    public class User : BaseEntity
    {
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        // store password hash, never store plain text
        public string PasswordHash { get; set; } = "";
        public string Role { get; set; } = nameof(UserRole.User);
        

        public User()
        {
        }

        public User(string username, string email, string passwordHash, string role = nameof(UserRole.User))
        {
            Username = username;
            UpdateEmail(email);
            UpdatePasswordHash(passwordHash);
            Role = role;
        }

        private void UpdatePasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ValidationException(nameof(PasswordHash), "Password hash cannot be empty");

            PasswordHash = passwordHash;
            UpdatedAt = DateTime.UtcNow;
        }

        private void UpdateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ValidationException(nameof(email),"Invalid email address");

            if (!IsValidEmail(email))
            {

            }

            Email = email;
            UpdatedAt = DateTime.UtcNow;
        }


        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
