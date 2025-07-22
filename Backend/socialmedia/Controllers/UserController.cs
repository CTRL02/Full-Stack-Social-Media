using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using socialmedia.Data;
using socialmedia.DTOs;
using socialmedia.Entities;
using socialmedia.Services;

namespace socialmedia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly ILogger<AccountController> _logger;
        private readonly ITokenService _tokenService;
        public UserController(ILogger<AccountController> logger, DBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            //edit this to only retrieve certain columns about user to show on user profile
            return await _context.Users.FindAsync(id);
        }

        [HttpGet("name/{username}")]
        public async Task<ActionResult<activeusersDto>> GetUserByUsername(string username)
        {
            //edit this to only retrieve certain columns about user to show on user profile
            return await _context.Users.
                Select(u => new activeusersDto 
                { 
                    username = u.UserName,
                    avatar = u.avatar 
                }).Where(u => u.username.ToLower() == username.ToLower()).FirstOrDefaultAsync();
        }

        [HttpGet("getusers")]
        public async Task<ActionResult<IEnumerable<activeusersDto>>> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new activeusersDto
                {
                    username = u.UserName,
                    avatar = u.avatar
                })
                .ToListAsync();

            return Ok(users);
        }


        [HttpGet("getuserswithterm/{searchTerm}")]
        public async Task<ActionResult<IEnumerable<activeusersDto>>> GetUsersBySearch(string searchTerm)
        {
            var users = await _context.Users
            .Where(u => u.UserName.ToLower().StartsWith(searchTerm.ToLower()))
             .Select(u => new activeusersDto
            {
                  username = u.UserName,
                  avatar = u.avatar
              })
          .ToListAsync();

            return Ok(users);
        }

    }
}
