using MediatR;

namespace Models.Teams.Commands
{
    public class DeleteTeamCommand : IRequest
    {
        public long TeamId { get; set; }
        public bool ValidatePermissions { get; set; } = true;
    }
}