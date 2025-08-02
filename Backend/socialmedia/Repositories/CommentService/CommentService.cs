using Microsoft.EntityFrameworkCore;
using socialmedia.Data;
using socialmedia.DTOs;
using socialmedia.Entities;

namespace socialmedia.Repositories.CommentService
{
    public class CommentService
    {
        private readonly DBContext _context;

        public CommentService(DBContext context)
        {
            _context = context;
        }

        public async Task<string> LeaveCommentAsync(int userId, CommentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Content))
                throw new ArgumentException("Comment content cannot be empty");

            var postExists = await _context.Posts.AnyAsync(p => p.Id == dto.PostId);
            if (!postExists)
                throw new KeyNotFoundException("Target post not found");

            if (dto.ParentCommentId != null)
            {
                var parent = await _context.Comments
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == dto.ParentCommentId.Value);

                if (parent == null)
                    throw new KeyNotFoundException("Parent comment not found");

                if (parent.PostId != dto.PostId)
                    throw new InvalidOperationException("Parent comment does not belong to the same post");
            }

            var comment = new Comment
            {
                Content = dto.Content.Trim(),
                CreatedAt = DateTime.UtcNow,
                AppUserId = userId,
                PostId = dto.PostId,
                ParentCommentId = dto.ParentCommentId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return "Comment posted successfully.";
        }
    }

}
