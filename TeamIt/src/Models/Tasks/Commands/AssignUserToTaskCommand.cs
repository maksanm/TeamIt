using MediatR;

namespace Models.Tasks.Commands
{
    public class AssignUserToTaskCommand : IRequest
    {
        public long ProjectId { get; set; }
        public long TaskId { get; set; }
        public string UserId { get; set; }
    }
}