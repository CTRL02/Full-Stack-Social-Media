using Microsoft.AspNetCore.Mvc;
using socialmedia.DTOs;
using socialmedia.Repositories.PostService;

namespace socialmedia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost]
        public async Task<ActionResult> CreatePost([FromBody] createPostDto postDto)
        {
            var post = await _postService.CreatePostAsync(postDto);
            return Ok(post);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(int id)
        {
            var result = await _postService.DeletePostAsync(id);
            if (!result) return NotFound("Post not found");
            return NoContent();
        }
    }

}
