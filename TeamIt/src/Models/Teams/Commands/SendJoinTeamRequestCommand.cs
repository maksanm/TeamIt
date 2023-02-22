using MediatR;

namespace Models.Teams.Commands
{
    public class SendJoinTeamRequestCommand : IRequest
    {
        public long TeamId { get; set; }
        public string UserId { get; set; }
    }
}