using MediatR;

namespace Models.Projects.Commands
{
    public class AddTeamToProjectCommand : IRequest
    {
        public long ProjectId { get; set; }
        public long TeamId { get; set; }
    }
}