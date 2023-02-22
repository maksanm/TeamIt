using MediatR;
using Models.Projects.Dto;
using Models.Teams.Dto;

namespace Models.Projects.Queries
{
    public class GetProjectMembersQuery : IRequest<ProjectMembersDto>
    {
        public long ProjectId { get; set; }
    }
}