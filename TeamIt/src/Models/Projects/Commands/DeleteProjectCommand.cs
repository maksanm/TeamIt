using MediatR;

namespace Models.Projects.Commands
{
    public class DeleteProjectCommand : IRequest
    {
        public long ProjectId { get; set; }
        public bool ValidatePermissions { get; set; } = true;
    }
}