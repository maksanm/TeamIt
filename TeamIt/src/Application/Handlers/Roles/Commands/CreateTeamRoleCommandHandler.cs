using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Providers;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Teams;
using Domain.Enums;
using MediatR;
using Models.Roles.Commands;

namespace Application.Handlers.Roles.Commands
{
    public class CreateTeamRoleCommandHandler : IRequestHandler<CreateTeamRoleCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionsProvider _permissionsProvider;
        private readonly IPermissionValidator _permissionValidator;

        private Team? _team;

        public CreateTeamRoleCommandHandler(
            IApplicationDbContext context,
            IPermissionsProvider permissionsProvider,
            IPermissionValidator permissionValidator)
        {
            _context = context;
            _permissionsProvider = permissionsProvider;
            _permissionValidator = permissionValidator;
        }

        public async Task<Unit> Handle(CreateTeamRoleCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            await _permissionValidator.ValidateTeamPermission(request.TeamId, PermissionEnum.TEAM_MANAGE_ROLE);

            var newRole = new Role()
            {
                Teams = new List<Team>() { _team! },
                Name = request.Name,
                Permissions = _permissionsProvider
                    .PermissionsWithIds(request.PermissionIds)
                    .ToList()
            };
            _team!.Roles.Add(newRole);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async Task ValidateRequest(CreateTeamRoleCommand request)
        {
            await ValidateTeam(request.TeamId);
            ValidateName(request.Name);
        }

        private async Task ValidateTeam(long teamId)
        {
            _team = await _context.Team.FindAsync(teamId);
            if (_team is null)
                throw new ValidationException("Team with provided id does not exist");
        }

        private void ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ValidationException("Role name cannot be empty");
        }
    }
}