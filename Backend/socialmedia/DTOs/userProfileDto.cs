namespace socialmedia.DTOs
{
    public class userProfileDto
    {
        public string username { get; set; }
        public string avatar { get; set; }
        public string bio { get; set; }
        public string title { get; set; }
        public int noOfPosts { get; set; }
        public int noOfFollowers { get; set; }
        public int noOfFollowing { get; set; }
        public List<string> socialLinks { get; set; }
    }
}
