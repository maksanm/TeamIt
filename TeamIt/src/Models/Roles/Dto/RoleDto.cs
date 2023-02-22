using Models.Mappings;
using Models.Permissions.Dto;

namespace Models.Roles.Dto
{
    public class RoleDto : IMapFrom<Domain.Entities.Role>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<PermissionDto> Permissions { get; set; }
    }
}