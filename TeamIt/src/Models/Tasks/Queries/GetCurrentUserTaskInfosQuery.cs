using MediatR;
using Models.Tasks.Dto;

namespace Models.Tasks.Queries
{
    public class GetCurrentUserTaskInfosQuery : IRequest<IList<TaskInfoDto>>
    {
        public long ProjectId { get; set; }
    }
}