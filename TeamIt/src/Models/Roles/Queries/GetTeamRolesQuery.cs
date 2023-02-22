using MediatR;
using Models.Roles.Dto;

namespace Models.Roles.Queries
{
    public class GetTeamRolesQuery : IRequest<IList<RoleDto>>
    {
        public long TeamId { get; set; }
    }
}