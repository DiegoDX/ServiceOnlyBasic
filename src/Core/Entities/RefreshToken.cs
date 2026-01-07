using Core.Common;

namespace Core.Entities
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt; 

        public RefreshToken()
        {
        }

        public RefreshToken(Guid userId, string token, DateTime expiresAt)
        {
            UserId = userId;
            Token = token;
            ExpiresAt = expiresAt;
            IsRevoked = false;
        }

        public void Revoke()
        {
            IsRevoked = true;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
