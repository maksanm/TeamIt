using MediatR;

namespace Models.Teams.Commands
{
    public class LeaveTeamCommand : IRequest
    {
        public long TeamId { get; set; }
    }
}