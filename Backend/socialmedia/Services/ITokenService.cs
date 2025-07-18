using socialmedia.Entities;

namespace socialmedia.Services
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
