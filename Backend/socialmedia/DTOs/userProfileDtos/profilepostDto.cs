namespace socialmedia.DTOs.userProfileDtos
{
    public class profilepostDto
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<profileimpressionDto> Impressions { get; set; }
        public List<profilecommentDto> Comments { get; set; }
    }
}
