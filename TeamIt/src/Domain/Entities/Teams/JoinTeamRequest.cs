namespace Domain.Entities.Teams
{
    public class JoinTeamRequest
    {
        public long Id { get; set; }
        public long TeamId { get; set; }
        public virtual Team Team { get; set; }
        public string UserToAddId { get; set; }
        public virtual User UserToAdd { get; set; }
        public string RequestSenderId { get; set; }
        public virtual User RequestSender { get; set; }
    }
}