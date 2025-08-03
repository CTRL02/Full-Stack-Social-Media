using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using socialmedia.Repositories.UserFollowService;
using socialmedia.Services;
using System.Security.Claims;
namespace socialmedia.Controllers
{
    public class UserFollowController:ControllerBase
    {
        private readonly IStringLocalizer<UserFollowController> stringLocalizer;
        private readonly IUserFollowService _userFollowService;

        public UserFollowController(IUserFollowService userFollowService, IStringLocalizer<UserFollowController> stringLocalizer)
        {
            _userFollowService = userFollowService;
            this.stringLocalizer = stringLocalizer;
        }


        [Authorize]
        [HttpPost("follow/{targetUsername}")]
        public async Task<IActionResult> FollowUser(string targetUsername)
        {
            var currentUsername = User.FindFirst(ClaimTypes.Name)?.Value;

            if (await _userFollowService.FollowUserAsync(currentUsername, targetUsername))
                return Ok(stringLocalizer["FollowSuccess"].Value);

            return BadRequest(stringLocalizer["FollowError"].Value);
        }

        [Authorize]
        [HttpDelete("unfollow/{targetUsername}")]
        public async Task<IActionResult> UnfollowUser(string targetUsername)
        {
            var currentUsername = User.FindFirst(ClaimTypes.Name)?.Value;

            if (await _userFollowService.UnfollowUserAsync(currentUsername, targetUsername))
                return Ok(stringLocalizer["UnfollowSuccess"].Value);

            return BadRequest(stringLocalizer["UnfollowError"].Value);
        }
    }
}
