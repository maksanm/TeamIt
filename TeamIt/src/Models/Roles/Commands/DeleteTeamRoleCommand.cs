using MediatR;

namespace Models.Roles.Commands
{
    public class DeleteTeamRoleCommand : IRequest
    {
        public long TeamId { get; set; }
        public long RoleId { get; set; }
    }
}