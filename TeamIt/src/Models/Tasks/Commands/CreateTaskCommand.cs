using MediatR;

namespace Models.Tasks.Commands
{
    public class CreateTaskCommand : IRequest
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DeadLine { get; set; }
        public long? ParentTaskId { get; set; }
        public string? AssignedUserId { get; set; }
    }
}