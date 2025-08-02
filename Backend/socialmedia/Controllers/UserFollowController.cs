using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using socialmedia.Repositories.UserFollowService;
using socialmedia.Services;
using System.Security.Claims;
namespace socialmedia.Controllers
{
    public class UserFollowController:ControllerBase
    {
        private readonly IUserFollowService _userFollowService;

        public UserFollowController(IUserFollowService userFollowService)
        {
            _userFollowService = userFollowService;
        }


        [Authorize]
        [HttpPost("follow/{targetUsername}")]
        public async Task<IActionResult> FollowUser(string targetUsername)
        {
            var currentUsername = User.FindFirst(ClaimTypes.Name)?.Value;

            if (await _userFollowService.FollowUserAsync(currentUsername, targetUsername))
                return Ok("User followed successfully");

            return BadRequest("Failed to follow user");
        }

        [Authorize]
        [HttpDelete("unfollow/{targetUsername}")]
        public async Task<IActionResult> UnfollowUser(string targetUsername)
        {
            var currentUsername = User.FindFirst(ClaimTypes.Name)?.Value;

            if (await _userFollowService.UnfollowUserAsync(currentUsername, targetUsername))
                return Ok("User unfollowed successfully");

            return BadRequest("Failed to unfollow user");
        }
    }
}
