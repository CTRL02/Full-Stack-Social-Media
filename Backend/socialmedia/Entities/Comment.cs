namespace socialmedia.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public int? ParentCommentId { get; set; } 
        public Comment ParentComment { get; set; }
        public ICollection<Impression> Impressions { get; set; } = new List<Impression>();
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();

    }
}
