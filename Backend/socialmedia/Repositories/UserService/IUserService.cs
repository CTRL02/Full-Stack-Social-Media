

using socialmedia.DTOs;
using socialmedia.Entities;

public interface IUserService
{
    Task<AppUser> GetUser(int id);
    Task<userProfileDto?> GetUserByUsername(string username);
    Task<IEnumerable<activeusersDto>> GetUsers();
    Task<IEnumerable<activeusersDto>> GetUsersBySearch(string searchTerm);

}
