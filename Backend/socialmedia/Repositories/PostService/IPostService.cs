using socialmedia.DTOs;
using socialmedia.Entities;

namespace socialmedia.Repositories.PostService
{
    public interface IPostService
    {
        Task<Post> CreatePostAsync(createPostDto postDto);
        Task<bool> DeletePostAsync(int postId);
    }

}
