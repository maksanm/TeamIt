using MediatR;
using Models.Teams.Dto;

namespace Models.Teams.Queries
{
    public class GetCurrentUserJoinTeamRequestsQuery : IRequest<IList<JoinTeamRequestDto>>
    {
    }
}
