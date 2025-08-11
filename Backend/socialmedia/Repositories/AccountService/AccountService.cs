// Services/AccountService.cs
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
    private readonly Cloudinary _cloudinary;

    public AccountService(DBContext context, ITokenService tokenService, IOptions<CloudinarySettings> config)
    {
        var account = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );
        
        _context = context;
        _tokenService = tokenService;
        _cloudinary = new Cloudinary(account);
    }

    public async Task<ActionResult<UserDto>> Register(RegisterDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.UserName.ToLower() == dto.Username.ToLower()))
            return new BadRequestObjectResult("UsernameExists");

        using var hmac = new HMACSHA512();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(dto.Photo.FileName, dto.Photo.OpenReadStream()),
            Folder = "profile_photos"
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        var user = new AppUser
        {
            UserName = dto.Username.ToLower(),
            avatar = uploadResult.SecureUrl.AbsoluteUri,
            PasswordSalt = hmac.Key,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
            Bio = dto.bio,
            Title = dto.title
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
