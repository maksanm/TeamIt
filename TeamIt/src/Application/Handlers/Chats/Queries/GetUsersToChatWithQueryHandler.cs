using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.ProjectManager;
using Domain.Entities.Teams;
using MediatR;
using Models.Chats.Dto;
using Models.Chats.Queries;
using Models.Projects.Dto;
using Models.Projects.Queries;
using Models.Teams.Dto;

namespace Application.Handlers.Chats.Queries
{
    public class GetUsersToChatWithQueryHandler : IRequestHandler<GetUsersToChatWithQuery, UsersToChatWithDto>
    {
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public GetUsersToChatWithQueryHandler(
            IIdentityService identityService,
            IMediator mediator,
            IMapper mapper)
        {
            _identityService = identityService;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<UsersToChatWithDto> Handle(GetUsersToChatWithQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _identityService.GetCurrentUserAsync();
            if (currentUser is null)
                throw new UnauthorizedException();

            var currentUserTeams = currentUser.TeamProfiles
                .Select(tp => tp.Team)
                .ToList();
            var currentUserProjects = currentUser.TeamProfiles
                .SelectMany(teamProfile => teamProfile.ProjectProfiles)
                .Select(tp => tp.Project)
                .ToList();
            RemoveUserProfiles(currentUser, currentUserTeams, currentUserProjects);

            var dto = new UsersToChatWithDto();
            dto.TeamUsers.AddRange(_mapper.Map<IList<TeamMembersDto>>(currentUserTeams));
            currentUserProjects.ForEach(async project =>
            {
                var projectMembersDto = await _mediator.Send(new GetProjectMembersQuery() { ProjectId = project.Id});
                dto.ProjectUsers.Add(projectMembersDto);
            });
            return dto;
        }

        private void RemoveUserProfiles(User user, List<Team> userTeams, List<Project> userProjects)
        {
            userTeams.ForEach(team =>
            {
                var currentUserProfile = team.Profiles.FirstOrDefault(profile => profile.User.Id == user.Id);
                team.Profiles.Remove(currentUserProfile!);
            });
            userProjects.ForEach(project =>
            {
                var currentUserProfile = project.Profiles.FirstOrDefault(profile => profile.User.Id == user.Id);
                project.Profiles.Remove(currentUserProfile!);
            });
        }
    }
}
