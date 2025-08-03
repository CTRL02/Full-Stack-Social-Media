// Services/AccountService.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using socialmedia.Data;
using socialmedia.DTOs;
using socialmedia.Entities;
using socialmedia.Repositories.AccountService;
using socialmedia.Services;
using System.Security.Cryptography;
using System.Text;

public class AccountService : IAccountService
{
    private readonly DBContext _context;
    private readonly ITokenService _tokenService;

    public AccountService(DBContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<ActionResult<UserDto>> Register(RegisterDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.UserName.ToLower() == dto.Username.ToLower()))
            return new BadRequestObjectResult("UsernameExists");

        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = dto.Username.ToLower(),
            avatar = dto.avatar,
            PasswordSalt = hmac.Key,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password))
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = _tokenService.CreateToken(user);

        return new OkObjectResult(new UserDto { Username = user.UserName, Token = token });
    }

    public async Task<ActionResult<UserDto>> Login(loginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == dto.Username.ToLower());
        if (user == null)
            return new UnauthorizedObjectResult("InvalidCredentials");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

        if (!computedHash.SequenceEqual(user.PasswordHash))
            return new UnauthorizedObjectResult("InvalidCredentials");

        var token = _tokenService.CreateToken(user);

        return new OkObjectResult(new UserDto { Username = user.UserName, Token = token });
    }
}
