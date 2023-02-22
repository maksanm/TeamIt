using MediatR;
using Microsoft.AspNetCore.Http;

namespace Models.Projects.Commands
{
    public class CreateProjectCommand : IRequest
    {
        public long CreatorTeamId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DeadLine { get; set; }
        public string LimitRoleName { get; set; }
        public List<int>? LimitRolePermissionIds { get; set; }
        public bool UseOwnHierarchy { get; set; }
        public IFormFile? Image { get; set; }
    }
}