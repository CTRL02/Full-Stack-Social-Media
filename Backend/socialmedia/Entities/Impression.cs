using System;

namespace socialmedia.Entities
{
    public class Impression
    {
        public int Id { get; set; }
        public ImpressionType Type { get; set; }
        public DateTime CreatedAt { get; set; }

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int? PostId { get; set; } 
        public Post Post { get; set; }

        public int? CommentId { get; set; }
        public Comment Comment { get; set; }
    }
    public enum ImpressionType
    {
        Like,
        Love,
        Sad,
        Angry,
        Care
    }
}
