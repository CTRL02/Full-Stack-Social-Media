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

        public async Task<bool> ToggleImpressionAsync(int userId, ImpressionDto dto)
        {
            if (!Enum.TryParse<ImpressionType>(dto.Type, true, out var type))
                throw new ArgumentException("InvalidImpressionType");

            if (dto.PostId == null && dto.CommentId == null)
                throw new ArgumentException("TargetIdRequired");

            if (dto.PostId != null && dto.CommentId != null)
                throw new ArgumentException("SingleTargetRequired");

            bool isPost = dto.PostId != null;
            int targetId = isPost ? dto.PostId.Value : dto.CommentId.Value;

            var exists = isPost
                ? await _context.Posts.AnyAsync(p => p.Id == targetId)
                : await _context.Comments.AnyAsync(c => c.Id == targetId);

            if (!exists)
                throw new KeyNotFoundException("TargetNotFound");

            var existing = await _context.Impressions.FirstOrDefaultAsync(i =>
                 i.AppUserId == userId &&
                 (isPost ? i.PostId == targetId : i.CommentId == targetId)
             );

            if (existing != null)
            {
                if (existing.Type == type)
                {
                    _context.Impressions.Remove(existing);
                    await _context.SaveChangesAsync();
                    return false; 
                }

                _context.Impressions.Remove(existing);
                await _context.SaveChangesAsync();
            }

            _context.Impressions.Add(new Impression
            {
                AppUserId = userId,
                CreatedAt = DateTime.UtcNow,
                Type = type,
                PostId = isPost ? targetId : null,
                CommentId = isPost ? null : targetId
            });

            await _context.SaveChangesAsync();
            return true; 
        }
    }
}
