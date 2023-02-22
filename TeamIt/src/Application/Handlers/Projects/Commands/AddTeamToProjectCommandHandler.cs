using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.ProjectManager;
using Domain.Entities.Teams;
using Domain.Enums;
using MediatR;
using Models.Projects.Commands;
using System.Net.Sockets;

namespace Application.Handlers.Projects.Commands
{
    public class AddTeamToProjectCommandHandler : IRequestHandler<AddTeamToProjectCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionValidator _permissionValidator;

        private Project? _project;
        private Team? _team;
        private IEnumerable<TeamProfile>? _usersToAddTeamProfiles;

        public AddTeamToProjectCommandHandler(
            IApplicationDbContext context,
            IPermissionValidator permissionValidator)
        {
            _context = context;
            _permissionValidator = permissionValidator;
        }

        public async Task<Unit> Handle(AddTeamToProjectCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            await _permissionValidator.ValidateTeamPermission(request.TeamId, PermissionEnum.TEAM_ADD_TO_PROJECT);
            await _permissionValidator.ValidateProjectManagerPermission(request.ProjectId, PermissionEnum.PM_ADD_TEAM);

            var newProfiles = _usersToAddTeamProfiles!
                .Select(tp => new ProjectProfile()
                {
                    Project = _project!,
                    TeamProfile = tp
                });
            _context.ProjectProfile.AddRange(newProfiles);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async System.Threading.Tasks.Task ValidateRequest(AddTeamToProjectCommand request)
        {
            await ValidateProject(request.ProjectId);
            await ValidateTeam(request.TeamId);
            ValidateUsersToAdd();
        }

        private async System.Threading.Tasks.Task ValidateProject(long projectId)
        {
            _project = await _context.Project.FindAsync(projectId);
            if (_project is null)
                throw new ValidationException("Project with provided id does not exist");
        }

        private async System.Threading.Tasks.Task ValidateTeam(long teamId)
        {
            _team = await _context.Team.FindAsync(teamId);
            if (_team is null)
                throw new ValidationException("Team with provided id does not exist");
        }

        private void ValidateUsersToAdd()
        {
            _usersToAddTeamProfiles = _team!.Profiles
                .Where(tp => !_project!.Profiles
                    .Any(pp => pp.User.Id == tp.User.Id));
            if (!_usersToAddTeamProfiles.Any())
                throw new ValidationException("All users from the team are already project members");
        }
    }
}