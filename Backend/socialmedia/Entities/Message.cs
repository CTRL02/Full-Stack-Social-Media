namespace socialmedia.Entities
{
    public class Message
    {
        public int Id { get; set; }

        public int SenderId { get; set; }
        public AppUser Sender { get; set; }
        public string RecipientUsername { get; set; }
        public string SenderUsername { get; set; }
        public int RecipientId { get; set; }
        public AppUser Recipient { get; set; }

        public string Content { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;
        public DateTime? ReadAt { get; set; }

        public bool SenderDeleted { get; set; } = false;
        public bool RecipientDeleted { get; set; } = false;
    }

}
