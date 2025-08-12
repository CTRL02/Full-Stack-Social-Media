using AutoMapper;
using Microsoft.EntityFrameworkCore;
using socialmedia.Data;
using socialmedia.DTOs;
using socialmedia.Entities;

namespace socialmedia.Repositories.CommentService
{
    public class CommentService
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;
        public CommentService(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> LeaveCommentAsync(int userId, CommentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Content))
                throw new ArgumentException("CommentContentEmpty");

            var postExists = await _context.Posts.AnyAsync(p => p.Id == dto.PostId);
            if (!postExists)
                throw new KeyNotFoundException("PostNotFound");

            if (dto.ParentCommentId != null)
            {
                var parent = await _context.Comments
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == dto.ParentCommentId.Value);

                if (parent == null)
                    throw new KeyNotFoundException("ParentCommentNotFound");

                if (parent.PostId != dto.PostId)
                    throw new InvalidOperationException("ParentCommentMismatch");
            }

            var comment = _mapper.Map<Comment>(dto);
            comment.Content = comment.Content.Trim();
            comment.CreatedAt = DateTime.UtcNow;
            comment.AppUserId = userId;


            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return "CommentPostedSuccess";
        }
    }

}
