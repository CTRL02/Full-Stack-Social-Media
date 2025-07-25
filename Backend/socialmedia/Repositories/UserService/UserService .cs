// Services/UserService.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using socialmedia.Data;
using socialmedia.DTOs;
using socialmedia.Entities;

public class UserService : IUserService
{
    private readonly DBContext _context;

    public UserService(DBContext context)
    {
        _context = context;
    }

    public async Task<AppUser> GetUser(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<userProfileDto?> GetUserByUsername(string username)
    {
            var query = _context.Users
        .Where(u => u.UserName.ToLower() == username.ToLower())
        .Select(u => new userProfileDto
        {
            username = u.UserName,
            avatar = u.avatar,
            bio = u.Bio,
            title = u.Title,
            noOfPosts = u.Posts.Count,
            noOfFollowers = u.Followers.Count,
            noOfFollowing = u.Following.Count,
            followers = u.Followers.Select(f => new activeusersDto
            {
                username = f.Follower.UserName,
                avatar = f.Follower.avatar
            }).ToList(),

            following = u.Following.Select(f => new activeusersDto
            {
                username = f.Followee.UserName,
                avatar = f.Followee.avatar
            }).ToList(),
            socialLinks = u.SocialLinks.Select(link => link.Url).ToList()
        });

            var sql = query.ToQueryString();
            Console.WriteLine(sql);

        return await _context.Users
            .Where(u => u.UserName.ToLower() == username.ToLower())
            .Select(u => new userProfileDto
            {
                username = u.UserName,
                avatar = u.avatar,
                bio = u.Bio,
                title = u.Title,
                noOfPosts = u.Posts.Count,
                noOfFollowers = u.Followers.Count,
                noOfFollowing = u.Following.Count,
                followers = u.Followers.Select(f => new activeusersDto
                {
                    username = f.Follower.UserName,
                    avatar = f.Follower.avatar
                }).ToList(),

                following = u.Following.Select(f => new activeusersDto
                {
                    username = f.Followee.UserName,
                    avatar = f.Followee.avatar
                }).ToList(),
                socialLinks = u.SocialLinks.Select(link => link.Url).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<activeusersDto>> GetUsers()
    {
        return await _context.Users
            .Select(u => new activeusersDto
            {
                username = u.UserName,
                avatar = u.avatar
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<activeusersDto>> GetUsersBySearch(string searchTerm)
    {
        return await _context.Users
            .Where(u => u.UserName.ToLower().StartsWith(searchTerm.ToLower()))
            .Select(u => new activeusersDto
            {
                username = u.UserName,
                avatar = u.avatar
            })
            .ToListAsync();
    }


  
}
