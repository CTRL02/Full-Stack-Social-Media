using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using socialmedia.DTOs;
using socialmedia.Repositories.CommentService;
using System.Security.Claims;

namespace socialmedia.Controllers
{
    public class CommentController:ControllerBase
    {
        private readonly CommentService _commentService;
        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }

        [Authorize]
        [HttpPost("comment")]
        public async Task<IActionResult> LeaveComment(CommentDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            try
            {
                var result = await _commentService.LeaveCommentAsync(userId, dto);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
