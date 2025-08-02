namespace socialmedia.DTOs
{
    public class CommentDto
    {
        public string Content { get; set; }
        public int PostId { get; set; }
        public int? ParentCommentId { get; set; }
    }
}
