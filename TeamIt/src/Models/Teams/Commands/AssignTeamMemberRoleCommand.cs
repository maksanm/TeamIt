using MediatR;

namespace Models.Teams.Commands
{
    public class AssignTeamMemberRoleCommand : IRequest
    {
        public string? UserId { get; set; }
        public long RoleId { get; set; }
        public long TeamId { get; set; }
    }
}