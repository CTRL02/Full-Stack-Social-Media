using Microsoft.EntityFrameworkCore;
using socialmedia.Data;
using socialmedia.Entities;

namespace socialmedia.Repositories.UserFollowService
{
    public class UserFollowService : IUserFollowService
    {
        private readonly DBContext _context;

        public UserFollowService(DBContext context)
        {
            _context = context;
        }

        public async Task<bool> FollowUserAsync(string currentUsername, string targetUsername)
        {
            if (currentUsername.ToLower() == targetUsername.ToLower())
                return false;

            var follower = await _context.Users
                .Include(u => u.Following)
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == currentUsername.ToLower());

            var followee = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == targetUsername.ToLower());

            if (follower == null || followee == null)
                return false;

            var alreadyFollowing = await _context.UserFollows.AsNoTracking()
                .AnyAsync(f => f.FollowerId == follower.Id && f.FolloweeId == followee.Id);

            if (alreadyFollowing)
                return false;

            var follow = new UserFollow
            {
                FollowerId = follower.Id,
                FolloweeId = followee.Id
            };

            _context.UserFollows.Add(follow);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UnfollowUserAsync(string currentUsername, string targetUsername)
        {
            var follower = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == currentUsername.ToLower());

            var followee = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == targetUsername.ToLower());

            if (follower == null || followee == null)
                return false;

            var follow = await _context.UserFollows
                .FirstOrDefaultAsync(f => f.FollowerId == follower.Id && f.FolloweeId == followee.Id);

            if (follow == null)
                return false;

            _context.UserFollows.Remove(follow);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
