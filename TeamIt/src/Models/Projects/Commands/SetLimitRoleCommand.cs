using MediatR;

namespace Models.Projects.Commands
{
    public class SetLimitRoleCommand : IRequest
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public List<int> PermissionIds { get; set; }
        public bool UseOwnHierarchy { get; set; }
    }
}