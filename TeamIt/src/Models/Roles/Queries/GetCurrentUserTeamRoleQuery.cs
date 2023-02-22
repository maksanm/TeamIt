using MediatR;
using Models.Roles.Dto;

namespace Models.Roles.Queries
{
    public class GetCurrentUserTeamRoleQuery : IRequest<RoleDto>
    {
        public long TeamId { get; set; }
    }
}