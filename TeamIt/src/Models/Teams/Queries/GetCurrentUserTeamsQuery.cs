using MediatR;
using Models.Teams.Dto;

namespace Models.Teams.Queries
{
    public class GetCurrentUserTeamsQuery : IRequest<IList<TeamDto>>
    {
    }
}