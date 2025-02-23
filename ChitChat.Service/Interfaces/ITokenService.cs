using ChitChat.Data.Entities;

namespace ChitChat.Service.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}
