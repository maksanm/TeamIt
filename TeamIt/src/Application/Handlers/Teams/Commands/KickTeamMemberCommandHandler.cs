using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities.Teams;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Teams.Commands;

namespace Application.Handlers.Teams.Commands
{
    public class KickTeamMemberCommandHandler : IRequestHandler<KickTeamMemberCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionValidator _permissionValidator;

        private TeamProfile? _kickedUserProfile;

        public KickTeamMemberCommandHandler(
            IApplicationDbContext context,
            IPermissionValidator permissionValidator)
        {
            _context = context;
            _permissionValidator = permissionValidator;
        }

        public async Task<Unit> Handle(KickTeamMemberCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);

            await _permissionValidator.ValidateTeamPermission(request.TeamId, PermissionEnum.TEAM_KICK_USER);

            _context.ChatProfile.RemoveRange(_kickedUserProfile!.ChatProfiles);
            _context.ProjectProfile.RemoveRange(_kickedUserProfile.ProjectProfiles);
            _context.TeamProfile.Remove(_kickedUserProfile);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async Task ValidateRequest(KickTeamMemberCommand request)
        {
            await ValidateKickedTeamMember(request);
            ValidateIfKickedUserIsNotTeamCreator();
        }

        private async Task ValidateKickedTeamMember(KickTeamMemberCommand request)
        {
            _kickedUserProfile = await _context.TeamProfile
                .FirstOrDefaultAsync(tp => tp.UserId == request.UserId
                                        && tp.TeamId == request.TeamId);
            if (_kickedUserProfile == default)
                throw new ValidationException("User is not a member of this team or team does not exist");
        }

        private void ValidateIfKickedUserIsNotTeamCreator()
        {
            if (_kickedUserProfile!.Team.CreatorUserId == _kickedUserProfile.UserId)
                throw new LackOfPermissionsException("Team creator kick is impossible");
        }
    }
}