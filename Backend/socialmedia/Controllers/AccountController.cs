using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using socialmedia.Data;
using socialmedia.DTOs;
using socialmedia.Entities;
using socialmedia.Repositories.AccountService;
using socialmedia.Services;
using System.Security.Cryptography;
using System.Text;

namespace socialmedia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto dto)
        {
            return await _accountService.Register(dto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(loginDto dto)
        {
            return await _accountService.Login(dto);
        }
    }
}
