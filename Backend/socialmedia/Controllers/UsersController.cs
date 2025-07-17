using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using socialmedia.Data;

namespace socialmedia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger, DBContext context)
        {
            _logger = logger;
            _context = context;
        }

      
    }
}
