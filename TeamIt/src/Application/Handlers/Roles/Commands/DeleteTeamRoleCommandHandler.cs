using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.Teams;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Models.Roles.Commands;

namespace Application.Handlers.Roles.Commands
{
    public class DeleteTeamRoleCommandHandler : IRequestHandler<DeleteTeamRoleCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionValidator _permissionValidator;

        private Team? _team;
        private Role? _role;

        public DeleteTeamRoleCommandHandler(
            IApplicationDbContext context,
            IPermissionValidator permissionValidator)
        {
            _context = context;
            _permissionValidator = permissionValidator;
        }

        public async Task<Unit> Handle(DeleteTeamRoleCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            await _permissionValidator.ValidateTeamPermission(request.TeamId, PermissionEnum.TEAM_MANAGE_ROLE);

            _team!.Roles.Remove(_role!);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async Task ValidateRequest(DeleteTeamRoleCommand request)
        {
            await ValidateTeam(request.TeamId);
            ValidateRole(request.RoleId);
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
    }
}