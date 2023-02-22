using MediatR;

namespace Models.Teams.Commands
{
    public class KickTeamMemberCommand : IRequest
    {
        public long TeamId { get; set; }
        public string UserId { get; set; }
    }
}