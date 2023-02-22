using MediatR;
using Models.Teams.Dto;

namespace Models.Projects.Queries
{
    public class GetTeamsToAddToProjectQuery : IRequest<IList<TeamDto>>
    {
        public string? Name { get; set; }
        public long ProjectId { get; set; }
    }
}