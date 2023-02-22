using MediatR;

namespace Models.Tasks.Commands
{
    public class DeleteTaskCommand : IRequest
    {
        public long ProjectId { get; set; }
        public long TaskId { get; set; }
    }
}