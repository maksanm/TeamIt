using Domain.Entities.Teams;
using Models.Mappings;
using Models.Roles.Dto;
using Models.Users.Dto;

namespace Models.Teams.Dto
{
    public class TeamMemberDto : IMapFrom<TeamProfile>
    {
        public UserDto User { get; set; }
        public RoleDto Role { get; set; }
    }
}