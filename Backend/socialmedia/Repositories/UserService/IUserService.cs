

using Microsoft.AspNetCore.Mvc;
using socialmedia.DTOs;
using socialmedia.Entities;

public interface IUserService
{
    Task<AppUser> GetUser(int id);
    //difference is this returns AppUser not a userprofileDto object as getuserbyusername
    Task<AppUser> GetUserByNameDuplicate(string username);

    Task<userProfileDto?> GetUserByUsername(string username);
    Task<IEnumerable<activeusersDto>> GetUsers();
    Task<IEnumerable<activeusersDto>> GetUsersBySearch(string searchTerm);
}
