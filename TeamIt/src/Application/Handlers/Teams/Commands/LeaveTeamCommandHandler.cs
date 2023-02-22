using Application.Common.Interfaces;
using Domain.Entities.Teams;
using MediatR;
using Models.Teams.Commands;
using System.Threading;

namespace Application.Handlers.Teams.Commands
{
    public class LeaveTeamCommandHandler : IRequestHandler<LeaveTeamCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;

        private TeamProfile? _memberProfile;

        public LeaveTeamCommandHandler(
            IApplicationDbContext context,
            IIdentityService identityService,
            IMediator mediator)
        {
            _context = context;
            _identityService = identityService;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(LeaveTeamCommand request, CancellationToken cancellationToken)
        {
            _memberProfile = await _identityService.GetCurrentUserTeamProfileAsync(request.TeamId);
            if (_memberProfile.UserId == _memberProfile.Team.CreatorUserId)
            {
                var teamCreatorChanged = await ChangeTeamCreatorOrDeleteTeam(request.TeamId);
                if (!teamCreatorChanged)
                    return Unit.Value;
            }
            _context.ProjectProfile.RemoveRange(_memberProfile!.ProjectProfiles);
            _context.ChatProfile.RemoveRange(_memberProfile.ChatProfiles);
            _context.TeamProfile.Remove(_memberProfile);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        // true if success, false if team was deleted
        public async Task<bool> ChangeTeamCreatorOrDeleteTeam(long teamId)
        {
            var newCreator = GetOtherUserWithHighestPermssions(_memberProfile!);
            if (newCreator is null)
            {
                await _mediator.Send(new DeleteTeamCommand() { TeamId = teamId, ValidatePermissions = false });
                return false;
            }
            else
            {
                await _mediator.Send(new ChangeTeamCreatorCommand() { TeamId = teamId, NewTeamCreatorUserId = newCreator.UserId });
                return true;
            }
        }

        private TeamProfile? GetOtherUserWithHighestPermssions(TeamProfile profile) =>
            profile.Team.Profiles
                .Where(p => p.UserId != profile.UserId)
                .OrderByDescending(p => p.Role.Permissions.Count)
                .FirstOrDefault();
    }
}