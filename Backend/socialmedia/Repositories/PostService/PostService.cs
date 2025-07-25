using Microsoft.EntityFrameworkCore;
using socialmedia.Data;
using socialmedia.DTOs;
using socialmedia.Entities;

namespace socialmedia.Repositories.PostService
{
    public class PostService : IPostService
    {
        private readonly DBContext _context;

        public PostService(DBContext context)
        {
            _context = context;
        }

        public async Task<Post> CreatePostAsync(createPostDto postDto)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == postDto.AppUserId);
            //if (!userExists)
            //    throw new ArgumentException("Invalid user ID: user does not exist.");

            var post = new Post
            {
                Content = postDto.Content,
                CreatedAt = DateTime.UtcNow,
                AppUserId = postDto.AppUserId
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<bool> DeletePostAsync(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post == null) return false;

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
