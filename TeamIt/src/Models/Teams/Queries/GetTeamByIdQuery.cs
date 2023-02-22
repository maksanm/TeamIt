using MediatR;
using Models.Teams.Dto;

namespace Models.Teams.Queries
{
    public class GetTeamByIdQuery : IRequest<TeamDto>
    {
        public long TeamId { get; set; }
    }
}