using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using socialmedia.DTOs;
using socialmedia.Repositories.PostService;
using System.Security.Claims;

namespace socialmedia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IStringLocalizer<PostController> stringLocalizer;
        public PostController(IPostService postService, IStringLocalizer<PostController> stringLocalizer)
        {
            _postService = postService;
            this.stringLocalizer = stringLocalizer;
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreatePost([FromBody] createPostDto postDto)
        {
            var post = await _postService.CreatePostAsync(postDto);
            return Ok(post);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var result = await _postService.DeletePostAsync(id, userId);

            if (!result) {
                var notFound = stringLocalizer.GetString("Post not found").Value;
                return NotFound(notFound); 
            
            } 
            return NoContent();
        }
    }

}
