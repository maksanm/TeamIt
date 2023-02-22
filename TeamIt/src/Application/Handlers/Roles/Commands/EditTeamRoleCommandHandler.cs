using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Providers;
using Domain.Entities;
using Domain.Entities.Teams;
using Domain.Enums;
using MediatR;
using Models.Roles.Commands;
using System.Linq;

namespace Application.Handlers.Roles.Commands
{
    public class EditTeamRoleCommandHandler : IRequestHandler<EditTeamRoleCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionsProvider _permissionsProvider;
        private readonly IPermissionValidator _permissionValidator;

        private Team? _team;
        private Role? _role;

        public EditTeamRoleCommandHandler(
            IApplicationDbContext context,
            IPermissionsProvider permissionsProvider,
            IPermissionValidator permissionValidator)
        {
            _context = context;
            _permissionValidator = permissionValidator;
            _permissionsProvider = permissionsProvider;
        }

        public async Task<Unit> Handle(EditTeamRoleCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            await _permissionValidator.ValidateTeamPermission(request.TeamId, PermissionEnum.TEAM_MANAGE_ROLE);

            _role!.Name = request.Name!;
            _role!.Permissions.Clear();
            _role.Permissions = _permissionsProvider
                .PermissionsWithIds(request.PermissionIds)
                .ToList();
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task ValidateRequest(EditTeamRoleCommand request)
        {
            await ValidateTeam(request.TeamId);
            ValidateRole(request.RoleId);
            ValidateRoleName(request.Name!);
        }

        private async Task ValidateTeam(long teamId)
        {
            _team = await _context.Team.FindAsync(teamId);
            if (_team is null)
                throw new ValidationException("Team with provided id does not exist");
        }

        private void ValidateRole(long roleId)
        {
            _role = _team!.Roles.FirstOrDefault(r => r.Id == roleId);
            if (_role == default)
                throw new ValidationException("There is no role with provided id in team");
        }

        private void ValidateRoleName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Role name cannot be empty");
        }
    }
}