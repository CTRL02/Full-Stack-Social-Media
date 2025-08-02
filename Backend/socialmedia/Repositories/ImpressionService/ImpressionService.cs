using Microsoft.EntityFrameworkCore;
using socialmedia.Data;
using socialmedia.DTOs;
using socialmedia.Entities;

namespace socialmedia.Repositories.ImpressionService
{
    public class ImpressionService
    {
        private readonly DBContext _context;

        public ImpressionService(DBContext context)
        {
            _context = context;
        }

        public async Task<string> ToggleImpressionAsync(int userId, ImpressionDto dto)
        {
            if (!Enum.TryParse<ImpressionType>(dto.Type, true, out var type))
                throw new ArgumentException("Invalid impression type");

            if (dto.PostId == null && dto.CommentId == null)
                throw new ArgumentException("Either PostId or CommentId must be provided");

            if (dto.PostId != null && dto.CommentId != null)
                throw new ArgumentException("Provide only one of PostId or CommentId");

            bool isPost = dto.PostId != null;
            int targetId = isPost ? dto.PostId.Value : dto.CommentId.Value;

            var exists = isPost
                ? await _context.Posts.AnyAsync(p => p.Id == targetId)
                : await _context.Comments.AnyAsync(c => c.Id == targetId);

            if (!exists)
                throw new KeyNotFoundException("Target post or comment does not exist");

            // check if impression exists already
            var existing = await _context.Impressions.FirstOrDefaultAsync(i =>
                 i.AppUserId == userId &&
                 (isPost ? i.PostId == targetId : i.CommentId == targetId)
             );


            if (existing != null)
            {
                if (existing.Type == type)
                {
                    // Same type = toggle off
                    _context.Impressions.Remove(existing);
                    await _context.SaveChangesAsync();
                    return "Impression removed.";
                }
                else
                {
                    // Different type = replace with new type
                    _context.Impressions.Remove(existing);
                    await _context.SaveChangesAsync();
                }
            }


            // Create new impression
            var newImpression = new Impression
            {
                AppUserId = userId,
                CreatedAt = DateTime.UtcNow,
                Type = type,
                PostId = isPost ? targetId : null,
                CommentId = isPost ? null : targetId
            };

            _context.Impressions.Add(newImpression);
            await _context.SaveChangesAsync();
            return "Impression added.";
        }
    }
}
