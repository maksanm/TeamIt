using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Teams;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Teams.Commands;

namespace Application.Handlers.Teams.Commands
{
    public class AssignTeamMemberRoleCommandHandler : IRequestHandler<AssignTeamMemberRoleCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionValidator _permissionValidator;

        private TeamProfile? _memberProfile;
        private Role? _memberRole;

        public AssignTeamMemberRoleCommandHandler(
            IApplicationDbContext context,
            IPermissionValidator permissionValidator)
        {
            _context = context;
            _permissionValidator = permissionValidator;
        }

        public async Task<Unit> Handle(AssignTeamMemberRoleCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            await _permissionValidator.ValidateTeamPermission(request.TeamId, PermissionEnum.TEAM_ASSIGN_ROLE);

            _memberProfile!.Role = _memberRole!;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async Task ValidateRequest(AssignTeamMemberRoleCommand request)
        {
            await ValidateTeamMember(request);
            await ValidateMemberRole(request);
        }

        private async Task ValidateTeamMember(AssignTeamMemberRoleCommand request)
        {
            _memberProfile = await _context.TeamProfile
                .FirstOrDefaultAsync(tp =>
                    tp.UserId == request.UserId &&
                    tp.TeamId == request.TeamId);
            if (_memberProfile == default)
                throw new ValidationException("User is not a member of this team or team does not exist");
        }

        private async Task ValidateMemberRole(AssignTeamMemberRoleCommand request)
        {
            _memberRole = await _context.Role.FirstOrDefaultAsync(r => r.Id == request.RoleId);
            if (_memberRole == default)
                throw new ValidationException($"Role with id: {request.RoleId} does not exist");
        }
    }
}