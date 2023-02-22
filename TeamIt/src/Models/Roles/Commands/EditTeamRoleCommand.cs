using MediatR;

namespace Models.Roles.Commands
{
    public class EditTeamRoleCommand : IRequest
    {
        public long TeamId { get; set; }
        public long RoleId { get; set; }
        public string? Name { get; set; }
        public List<int> PermissionIds { get; set; }
    }
}