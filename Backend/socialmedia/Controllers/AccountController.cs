using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
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
        private readonly IStringLocalizer<AccountController> _localizer;
        public AccountController(IAccountService accountService, IStringLocalizer<AccountController> localizer)
        {
            _accountService = accountService;
            _localizer = localizer;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto dto)
        {
            var result = await _accountService.Register(dto);
            
            if (result.Result is BadRequestObjectResult badRequest)
            {
                return BadRequest(_localizer["Username Already Exists"].Value);
            }

            

            return result;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(loginDto dto)
        {
            var result = await _accountService.Login(dto);

            if (result.Result is UnauthorizedObjectResult unauthorized)
            {
                return Unauthorized(_localizer["InvalidUsernameOrPassword"].Value);
            }
            return result;
        }
    }
}
