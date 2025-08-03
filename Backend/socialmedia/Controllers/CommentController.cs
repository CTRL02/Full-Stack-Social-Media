using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using socialmedia.DTOs;
using socialmedia.Repositories.CommentService;
using System.Security.Claims;

namespace socialmedia.Controllers
{
    public class CommentController:ControllerBase
    {
        private readonly IStringLocalizer<CommentController> _localizer;
        private readonly CommentService _commentService;
        public CommentController(CommentService commentService, IStringLocalizer<CommentController> localizer)
        {
            _commentService = commentService;
            _localizer = localizer;
        }

        [Authorize]
        [HttpPost("comment")]
        public async Task<IActionResult> LeaveComment(CommentDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            try
            {
                await _commentService.LeaveCommentAsync(userId, dto);
                return Ok(new { message = _localizer["CommentPostedSuccess"].Value });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = _localizer["CommentContentEmpty"].Value });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    error = ex.Message.Contains("post")
                        ? _localizer["PostNotFound"].Value
                        : _localizer["ParentCommentNotFound"].Value
                });
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new { error = _localizer["ParentCommentMismatch"].Value });
            }
           
        }
    }
}
