using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.ProjectManager;
using Domain.Enums;
using MediatR;
using Models.Projects.Commands;

namespace Application.Handlers.Projects.Commands
{
    public class KickTeamFromProjectCommandHandler : IRequestHandler<KickTeamFromProjectCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionValidator _permissionValidator;

        private Project? _project;

        public KickTeamFromProjectCommandHandler(
            IApplicationDbContext context,
            IPermissionValidator permissionValidator)
        {
            _context = context;
            _permissionValidator = permissionValidator;
        }

        public async Task<Unit> Handle(KickTeamFromProjectCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            await _permissionValidator.ValidateProjectManagerPermission(request.ProjectId, PermissionEnum.PM_KICK_TEAM);

            _project!.Profiles
                .Where(pp => pp.TeamProfile.TeamId == request.TeamId)
                .ToList()
                .ForEach(pp =>
                {
                    _context.ChatProfile.RemoveRange(pp.ChatProfiles);
                    _context.ProjectProfile.Remove(pp);
                });
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async System.Threading.Tasks.Task ValidateRequest(KickTeamFromProjectCommand request)
        {
            await ValidateProject(request.ProjectId);
            ValidateKickedTeamMembers(request.TeamId);
            ValidateProjectTeamCreator(request.TeamId);
        }

        private async System.Threading.Tasks.Task ValidateProject(long projectId)
        {
            _project = await _context.Project.FindAsync(projectId);
            if (_project is null)
                throw new ValidationException("Project with provided id does not exist");
        }

        private void ValidateKickedTeamMembers(long teamId)
        {
            var anyTeamMemberWasAdded = _project!.Profiles.Any(pp => pp.TeamProfile.TeamId == teamId);
            if (!anyTeamMemberWasAdded)
                throw new ValidationException("No members of the team with specified id were added to the project");
        }

        private void ValidateProjectTeamCreator(long teamId)
        {
            if (teamId == _project!.CreatorTeamId)
                throw new ValidationException("Project creator team cannot be kicked from the project");
        }
    }
}