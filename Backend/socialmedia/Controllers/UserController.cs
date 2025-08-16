// Controllers/UserController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using socialmedia.DTOs;
using socialmedia.Entities;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var user = await _userService.GetUser(id);
        return user != null ? Ok(user) : NotFound();
    }

    [HttpGet("name/duplicate/{username}")]
    public async Task<ActionResult<AppUser>> GetUserByNameDuplicate(string username)
    {
        var user = await _userService.GetUserByNameDuplicate(username);
        return user != null ? Ok(user) : NotFound();

    }

    [HttpGet("name/{username}")]
    public async Task<ActionResult<userProfileDto>> GetUserByUsername(string username)
    {
        var userDto = await _userService.GetUserByUsername(username);
        return userDto != null ? Ok(userDto) : NotFound();
    }

    [HttpGet("getusers")]
    public async Task<ActionResult<IEnumerable<activeusersDto>>> GetUsers()
    {
        var users = await _userService.GetUsers();
        return Ok(users);
    }

    [HttpGet("getuserswithterm/{searchTerm}")]
    public async Task<ActionResult<IEnumerable<activeusersDto>>> GetUsersBySearch(string searchTerm)
    {
        var users = await _userService.GetUsersBySearch(searchTerm);
        return Ok(users);
    }

   
}
