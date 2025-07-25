namespace socialmedia.Entities
{
    public class UserFollow
    {
        public int FollowerId { get; set; }
        public AppUser Follower { get; set; }

        public int FolloweeId { get; set; }
        public AppUser Followee { get; set; }
    }
}
