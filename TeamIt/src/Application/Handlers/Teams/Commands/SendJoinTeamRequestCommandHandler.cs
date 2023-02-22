using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Providers;
using Domain.Entities;
using Domain.Entities.ProjectManager;
using Domain.Entities.Teams;
using Domain.Enums;
using MediatR;
using Models.Teams.Commands;

namespace Application.Handlers.Teams.Commands
{
    public class SendJoinTeamRequestCommandHandler : IRequestHandler<SendJoinTeamRequestCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionValidator _permissionValidator;
        private readonly IIdentityService _identityService;

        private User? _user;
        private User? _currenUser;
        private Team? _team;

        public SendJoinTeamRequestCommandHandler(
            IApplicationDbContext context,
            IPermissionValidator permissionValidator,
            IIdentityService identityService)
        {
            _context = context;
            _permissionValidator = permissionValidator;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(SendJoinTeamRequestCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);
            await _permissionValidator.ValidateTeamPermission(request.TeamId, PermissionEnum.TEAM_ADD_USER);

            _currenUser = await _identityService.GetCurrentUserAsync();
            var joinTeamRequest = CreateJoinTeamRequest();
            _user!.JoinTeamRequests.Add(joinTeamRequest);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private void ValidateRequest(SendJoinTeamRequestCommand request)
        {
            ValidateTeam(request.TeamId);
            ValidateUser(request.UserId);
            ValdiateIfUserIsAlreadyAdded(request.UserId);
        }

        private JoinTeamRequest CreateJoinTeamRequest() =>
            new JoinTeamRequest()
            {
                Team = _team!,
                UserToAdd = _user!,
                RequestSender = _currenUser!
            };

        private void ValidateTeam(long teamId)
        {
            _team = _context.Team.Find(teamId);
            if (_team is null)
                throw new ValidationException("Team with provided id does not exist");
        }

        private void ValidateUser(string userId)
        {
            _user = _context.User.Find(userId);
            if (_user is null)
                throw new ValidationException("User with provided id does not exist");
        }

        private void ValdiateIfUserIsAlreadyAdded(string userId)
        {
            var isAlreadyAdded = _team!.Profiles.Any(p => p.User.Id == userId);
            if (isAlreadyAdded)
                throw new ValidationException("User is already member of the provided team");
        }
    }
}