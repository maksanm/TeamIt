using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.ProjectManager;
using Domain.Enums;
using MediatR;
using Models.Projects.Commands;

namespace Application.Handlers.Projects.Commands
{
    public class LeaveProjectCommandHandler : IRequestHandler<LeaveProjectCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionValidator _permissionValidator;

        private Project? _project;

        public LeaveProjectCommandHandler(
            IApplicationDbContext context,
            IPermissionValidator permissionValidator)
        {
            _context = context;
            _permissionValidator = permissionValidator;
        }

        public async Task<Unit> Handle(LeaveProjectCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            await _permissionValidator.ValidateProjectManagerPermission(request.TeamId, PermissionEnum.TEAM_LEAVE_PROJECT);

            var leavingTeamProfiles = _project!.Profiles.Where(pp => pp.TeamProfile.TeamId == request.TeamId);
            _context.ProjectProfile.RemoveRange(leavingTeamProfiles);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async System.Threading.Tasks.Task ValidateRequest(LeaveProjectCommand request)
        {
            await ValidateProject(request.ProjectId);
            ValidateProjectTeamCreator(request.TeamId);
        }

        private async System.Threading.Tasks.Task ValidateProject(long projectId)
        {
            _project = await _context.Project.FindAsync(projectId);
            if (_project is null)
                throw new ValidationException("Project with provided id does not exist");
        }

        private void ValidateProjectTeamCreator(long teamId)
        {
            if (_project!.CreatorTeamId == teamId)
                throw new ValidationException("Project creator team cannot leave the project. Consider deleting it instead");
        }
    }
}