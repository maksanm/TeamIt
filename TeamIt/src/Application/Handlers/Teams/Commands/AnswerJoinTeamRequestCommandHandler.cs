using Application.Common.Interfaces;
using Application.Common.Providers;
using Domain.Entities.Teams;
using MediatR;
using Models.Teams.Commands;
using Application.Common.Exceptions;
using Domain.Entities.ProjectManager;

namespace Application.Handlers.Teams.Commands
{
    public class AnswerJoinTeamRequestCommandHandler : IRequestHandler<AnswerJoinTeamRequestCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IBasicRolesProvider _basicRolesProvider;
        private readonly IIdentityService _identityService;

        private JoinTeamRequest? _joinTeamRequest;
        private TeamProfile? _newTeamProfile;

        public AnswerJoinTeamRequestCommandHandler(
            IApplicationDbContext context,
            IBasicRolesProvider basicRolesProvider,
            IIdentityService identityService)
        {
            _context = context;
            _basicRolesProvider = basicRolesProvider;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(AnswerJoinTeamRequestCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);

            if (request.Approve)
            {
                CreateTeamProfile();
                CreateProfilesForTeamProjects();
            }
            _context.JoinTeamRequest.Remove(_joinTeamRequest!);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async System.Threading.Tasks.Task ValidateRequest(AnswerJoinTeamRequestCommand request)
        {
            var currentUser = await _identityService.GetCurrentUserAsync();
            _joinTeamRequest = currentUser.JoinTeamRequests.FirstOrDefault(r => r.Id ==request.RequestId);
            if (_joinTeamRequest is null)
                throw new ValidationException("Request with provided id doesn't wait for user response");
        }

        private void CreateTeamProfile()
        {
            _newTeamProfile = new TeamProfile()
            {
                User = _joinTeamRequest!.UserToAdd,
                Team = _joinTeamRequest.Team,
                Role = _basicRolesProvider.GuestRole()
            };
            _joinTeamRequest.Team.Profiles.Add(_newTeamProfile);
            if (!_joinTeamRequest.Team.Roles.Contains(_newTeamProfile.Role))
                _joinTeamRequest.Team.Roles.Add(_newTeamProfile.Role);
        }

        private void CreateProfilesForTeamProjects()
        {
            _joinTeamRequest!.Team
                .CreatorProfile
                .ProjectProfiles
                .ToList()
                .ForEach(pp =>
                {
                    var newProjectProfile = new ProjectProfile()
                    {
                        Project = pp.Project,
                        TeamProfile = _newTeamProfile!
                    };
                    pp.Project.Profiles.Add(newProjectProfile);
                });
        }
    }
}
