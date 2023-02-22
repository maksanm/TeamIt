using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Providers;
using Domain.Entities;
using Domain.Entities.Teams;
using MediatR;
using Models.Teams.Commands;

namespace Application.Handlers.Teams.Commands
{
    public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IBasicRolesProvider _basicRolesProvider;
        private readonly IImageService _imageService;

        public CreateTeamCommandHandler(
            IApplicationDbContext context,
            IIdentityService identityService,
            IBasicRolesProvider basicRolesProvider,
            IImageService imageService)
        {
            _context = context;
            _identityService = identityService;
            _basicRolesProvider = basicRolesProvider;
            _imageService = imageService;
        }

        public async Task<Unit> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            ValidateReqest(request);

            var newTeam = await CreateNewTeam(request);
            _context.Team.Add(newTeam);
            await _context.SaveChangesAsync(cancellationToken);
            if (request.Image is not null)
                await _imageService.SetTeamPicture(newTeam.Id, request.Image);
            return Unit.Value;
        }

        private void ValidateReqest(CreateTeamCommand request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ValidationException("Team name must be provided");
            if (_context.Team.Any(t => t.Name.Equals(request.Name)))
                throw new ValidationException("Team name should be unique");
        }

        private async Task<Team> CreateNewTeam(CreateTeamCommand request)
        {
            var team = new Team()
            {
                Name = request.Name,
                Roles = new List<Role>(),
                Profiles = new List<TeamProfile>(),
            };
            var currentUserTeamProfile = await CreateCurrentUserTeamCreatorProfile(team);
            team.CreatorUser = currentUserTeamProfile.User;
            team.Profiles.Add(currentUserTeamProfile);
            if (!team.Roles.Contains(currentUserTeamProfile.Role))
                team.Roles.Add(currentUserTeamProfile.Role);
            return team;
        }

        private async Task<TeamProfile> CreateCurrentUserTeamCreatorProfile(Team team)
        {
            var currentUser = await _identityService.GetCurrentUserAsync();
            var teamProfile = new TeamProfile()
            {
                User = currentUser,
                Team = team,
                Role = _basicRolesProvider.TeamCreatorRole()
            };
            return teamProfile;
        }
    }
}