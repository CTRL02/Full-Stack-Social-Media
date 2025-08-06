namespace socialmedia.DTOs.userProfileDtos
{
    public class profilecommentDto
    {
        public int id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string username { get; set; }
        public string avatar { get; set; }
        public List<profileimpressionDto> Impressions { get; set; }
        public List<profilecommentDto> Replies { get; set; }
    }
}
