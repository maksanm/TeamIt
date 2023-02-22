using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Chats;
using Domain.Entities.Teams;
using Domain.Enums;
using MediatR;
using Models.Chats.Dto;
using Models.Chats.Queries;
using Models.Projects.Dto;
using Models.Teams.Dto;

namespace Application.Handlers.Chats.Queries
{
    public class GetUsersToAddToChatQueryHandler : IRequestHandler<GetUsersToAddToChatQuery, UsersToChatWithDto>
    {
        private readonly IIdentityService _identityService;
        private readonly IPermissionValidator _permissionValidator;
        private readonly IMapper _mapper;

        public GetUsersToAddToChatQueryHandler(
            IIdentityService identityService,
            IPermissionValidator permissionValidator,
            IMapper mapper)
        {
            _identityService = identityService;
            _permissionValidator = permissionValidator;
            _mapper = mapper;
        }

        public async Task<UsersToChatWithDto> Handle(GetUsersToAddToChatQuery request, CancellationToken cancellationToken)
        {
            await _permissionValidator.ValidateChatPermission(request.ChatId, PermissionEnum.CHAT_ADD_USER);

            var currentUserChatProfile = await _identityService.GetCurrentUserChatProfileAsync(request.ChatId);
            var chat = currentUserChatProfile.Chat;
            var dto = new UsersToChatWithDto();
            if (chat.BaseTeam is not null)
                AddBaseTeamMembers(chat, dto);
            else if (chat.BaseProject is not null)
                AddBaseProjectMembers(chat, dto);
            return dto;
        }

        private void AddBaseTeamMembers(Chat chat, UsersToChatWithDto dto)
        {
            var teamWithoutChatMembers = chat.BaseTeam!.Profiles
                .Where(teamProfile => !teamProfile.ChatProfiles
                    .Any(chatProfile => chatProfile.ChatId == chat.Id))
                .ToList();
            dto.TeamUsers.Add(new TeamMembersDto()
            {
                TeamInfo = _mapper.Map<TeamDto>(chat.BaseTeam),
                Members = _mapper.Map<List<TeamMemberDto>>(teamWithoutChatMembers),
            });
        }

        private void AddBaseProjectMembers(Chat chat, UsersToChatWithDto dto)
        {
            var teamsWithoutChatMembers = GetProjectTeamsWithoutChatMembers(chat);
            dto.ProjectUsers.Add(
                new ProjectMembersDto()
                {
                    ProjectInfo = _mapper.Map<ProjectInfoDto>(chat.BaseProject),
                    Members = _mapper.Map<List<TeamMembersDto>>(teamsWithoutChatMembers)
                });
        }

        private IEnumerable<Team> GetProjectTeamsWithoutChatMembers(Chat chat)
        {
            var teamsWithoutChatMembers = chat.BaseProject!
                .GetProjectTeamsList();
            teamsWithoutChatMembers.ForEach(team =>
            {
                team.Profiles = team.Profiles
                    .Where(teamProfile => !chat.Profiles
                        .Any(chatProfile => chatProfile.User.Id == teamProfile.User.Id))
                    .ToList();
            });
            return teamsWithoutChatMembers;
        }
    }
}
