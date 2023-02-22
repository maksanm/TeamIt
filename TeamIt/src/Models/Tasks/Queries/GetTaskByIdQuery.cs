using MediatR;
using Models.Tasks.Dto;

namespace Models.Tasks.Queries
{
    public class GetTaskByIdQuery : IRequest<TaskDto>
    {
        public long ProjectId { get; set; }
        public long TaskId { get; set; }
    }
}