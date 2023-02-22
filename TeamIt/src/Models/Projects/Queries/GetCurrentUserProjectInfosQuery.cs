using MediatR;
using Models.Projects.Dto;

namespace Models.Projects.Queries
{
    public class GetCurrentUserProjectInfosQuery : IRequest<IList<ProjectInfoDto>>
    {
    }
}