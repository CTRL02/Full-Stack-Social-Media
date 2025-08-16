// Services/UserService.cs
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using socialmedia.Data;
using socialmedia.DTOs;
using socialmedia.DTOs.userProfileDtos;
using socialmedia.Entities;

public class UserService : IUserService
{
    private readonly DBContext _context;
    private readonly IMapper _mapper;
    public UserService(DBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    

    public async Task<AppUser> GetUser(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<AppUser> GetUserByNameDuplicate(string username)
    {
        return await _context.Users.Where(u => u.UserName.ToLower() == username.ToLower()).FirstOrDefaultAsync();
    }

    public async Task<userProfileDto?> GetUserByUsername(string username)
    {

        return await _context.Users
            .Where(u => u.UserName.ToLower() == username.ToLower())
            .ProjectTo<userProfileDto>(_mapper.ConfigurationProvider) // AutoMapper magic
            .FirstOrDefaultAsync();


    }

    public async Task<IEnumerable<activeusersDto>> GetUsers()
    {
        return await _context.Users
            .ProjectTo<activeusersDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<IEnumerable<activeusersDto>> GetUsersBySearch(string searchTerm)
    {
        return await _context.Users
            .Where(u => u.UserName.ToLower().StartsWith(searchTerm.ToLower()))
            .ProjectTo<activeusersDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

}
