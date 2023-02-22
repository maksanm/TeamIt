using MediatR;

namespace Models.Roles.Commands
{
    public class CreateTeamRoleCommand : IRequest
    {
        public long TeamId { get; set; }
        public string Name { get; set; }
        public List<int> PermissionIds { get; set; }
    }
}