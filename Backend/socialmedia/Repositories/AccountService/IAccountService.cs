using Microsoft.AspNetCore.Mvc;
using socialmedia.DTOs;

namespace socialmedia.Repositories.AccountService
{
    public interface IAccountService
    {
        Task<ActionResult<UserDto>> Register(RegisterDto dto);
        Task<ActionResult<UserDto>> Login(loginDto dto);
    }
}
