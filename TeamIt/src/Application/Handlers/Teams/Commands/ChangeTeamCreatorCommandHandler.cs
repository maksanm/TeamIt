using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Teams;
using MediatR;
using Models.Teams.Commands;

namespace Application.Handlers.Teams.Commands
{
    public class ChangeTeamCreatorCommandHandler : IRequestHandler<ChangeTeamCreatorCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        private Team? _team;
        private User? _newTeamCreator;

        public ChangeTeamCreatorCommandHandler(
            IApplicationDbContext context,
            IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(ChangeTeamCreatorCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);

            _team!.CreatorUser = _newTeamCreator!;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task ValidateRequest(ChangeTeamCreatorCommand request)
        {
            await ValidateTeam(request.TeamId);
            await ValidateNewTeamCreator(request.NewTeamCreatorUserId);
            await ValidateIfCurrentUserIsTeamCreator();
        }

        private async Task ValidateTeam(long teamId)
        {
            _team = await _context.Team.FindAsync(teamId);
            if (_team is null)
                throw new ValidationException("Team with provided id does not exist");
        }

        private async Task ValidateNewTeamCreator(string newTeamCreatorUserId)
        {
            _newTeamCreator = await _context.User.FindAsync(newTeamCreatorUserId);
            if (_newTeamCreator is null)
                throw new ValidationException("User with provided id does not exist");
        }

        private async Task ValidateIfCurrentUserIsTeamCreator()
        {
            var currentUser = await _identityService.GetCurrentUserAsync();
            if (currentUser.Id != _team!.CreatorUserId)
                throw new LackOfPermissionsException("User is not allowed to change team creator");
        }
    }
}