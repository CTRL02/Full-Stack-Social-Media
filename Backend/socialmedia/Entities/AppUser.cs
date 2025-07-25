namespace socialmedia.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? avatar { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }

        public string? Bio { get; set; }
        public string? Title { get; set; }

        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<UserFollow> Followers { get; set; } = new List<UserFollow>();
        public ICollection<UserFollow> Following { get; set; } = new List<UserFollow>();
        public ICollection<SocialLink> SocialLinks { get; set; } = new List<SocialLink>();


    }
}
