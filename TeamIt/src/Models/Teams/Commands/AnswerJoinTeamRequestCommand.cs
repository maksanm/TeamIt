using MediatR;

namespace Models.Teams.Commands
{
    public class AnswerJoinTeamRequestCommand : IRequest
    {
        public long RequestId { get; set; }
        public bool Approve { get; set; }
    }
}