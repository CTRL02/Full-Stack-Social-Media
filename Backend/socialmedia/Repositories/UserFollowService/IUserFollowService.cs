namespace socialmedia.Repositories.UserFollowService
{
    public interface IUserFollowService
    {
        Task<bool> FollowUserAsync(string currentUsername, string targetUsername);
        Task<bool> UnfollowUserAsync(string currentUsername, string targetUsername);
    }
}
