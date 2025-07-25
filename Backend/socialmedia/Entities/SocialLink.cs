namespace socialmedia.Entities
{
    public class SocialLink
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }



    }
}
