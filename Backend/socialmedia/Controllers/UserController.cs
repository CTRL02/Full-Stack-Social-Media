using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using socialmedia.Data;
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

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
