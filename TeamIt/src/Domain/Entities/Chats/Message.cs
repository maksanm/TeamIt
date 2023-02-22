namespace Domain.Entities.Chats
{
    public class Message
    {
        public long Id { get; set; }
        public string? Text { get; set; }
        public DateTime Date { get; set; }
        public long ChatId { get; set; }
        public virtual Chat Chat { get; set; }
        public long? AttachedImageId { get; set; }
        public virtual Picture? AttachedImage { get; set; }
        public long? SenderProfileId { get; set; }
        public virtual ChatProfile? SenderProfile { get; set; }
    }
}
