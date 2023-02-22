using Domain.Enums;
using MediatR;
using Models.Enums;

namespace Models.Tasks.Commands
{
    public class EditTaskCommand : IRequest
    {
        public long ProjectId { get; set; }
        public long TaskId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DeadLine { get; set; }
        public TaskStateEnumDto? State { get; set; }
    }
}