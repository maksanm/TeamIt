using MediatR;
using Models.Projects.Dto;

namespace Models.Projects.Queries
{
    public class GetProjectByIdQuery : IRequest<ProjectDto>
    {
        public long ProjectId { get; set; }
    }
}