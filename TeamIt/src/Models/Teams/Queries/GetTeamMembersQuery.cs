using MediatR;
using Models.Teams.Dto;
using Models.Users.Dto;

namespace Models.Users.Commands
{
    public class GetTeamMembersQuery : IRequest<IList<TeamMemberDto>>
    {
        public long TeamId { get; set; }
    }
}