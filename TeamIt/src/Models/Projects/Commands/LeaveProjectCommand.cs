using MediatR;

namespace Models.Projects.Commands
{
    public class LeaveProjectCommand : IRequest
    {
        public long ProjectId { get; set; }
        public long TeamId { get; set; }
    }
}