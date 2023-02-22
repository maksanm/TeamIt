using Application.Common.Interfaces;
using Application.Common.Providers;
using Domain.Entities.ProjectManager;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Models.Projects.Commands;
using Models.Chats.Commands;
using Application.Common.Exceptions;
using Domain.Entities.Teams;
using Domain.Entities.Chats;

namespace Application.Handlers.Chats.Commands
{
    public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IImageService _imageService;

        private Team? _baseTeam;
        private Project? _baseProject;
        private User? _userToChatWith;

        public CreateChatCommandHandler(
            IApplicationDbContext context,
            IIdentityService identityService,
            IImageService imageService)
        {
            _context = context;
            _identityService = identityService;
            _imageService = imageService;
        }

        public async Task<Unit> Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);

            var newChat = await CreateNewChat(request);
            _context.Chat.Add(newChat);
            await _context.SaveChangesAsync(cancellationToken);
            if (request.ChatPicture is not null)
                await _imageService.SetChatPicture(newChat.Id, request.ChatPicture);
            return Unit.Value;
        }

        private async System.Threading.Tasks.Task ValidateRequest(CreateChatCommand request)
        {
            ValidateChatName(request.Name);
            if (request.TeamId is null)
                await ValidateChatBaseProject(request.ProjectId);
            else if (request.ProjectId is null)
                await ValidateChatBaseTeam(request.TeamId);
            else
                throw new ValidationException("Either base team or project id must be provided");
            ValidateUser(request.UserId);
        }

        private async Task<Chat> CreateNewChat(CreateChatCommand request)
        {
            var chat = new Chat()
            {
                Name = request.Name,
                BaseTeam = _baseTeam!,
                BaseProject = _baseProject!,
                Profiles = new List<ChatProfile>()
            };
            var currentUser = await _identityService.GetCurrentUserAsync();
            chat.Profiles.Add(CreateUserChatProfile(currentUser.Id, chat));
            chat.Profiles.Add(CreateUserChatProfile(request.UserId, chat));
            return chat;
        }

        private void ValidateChatName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Chat name must be provided");
        }

        private void ValidateUser(string userId)
        {
            if (_baseTeam is not null)
                _userToChatWith = _baseTeam?.Profiles
                    .Select(profile => profile.User)
                    .FirstOrDefault(user => user.Id == userId);
            else if (_baseProject is not null)
                _userToChatWith = _baseProject?.Profiles
                    .Select(profile => profile.User)
                    .FirstOrDefault(user => user.Id == userId);

            if (_userToChatWith == default)
                throw new ValidationException("User with provided id is not a member of the chat base team or project");
        }

        private async System.Threading.Tasks.Task ValidateChatBaseProject(long? projectId)
        {
            _baseProject = await _context.Project.FindAsync(projectId);
            if (_baseProject is null)
                throw new ValidationException("Project with provided id does not exist");
        }

        private async System.Threading.Tasks.Task ValidateChatBaseTeam(long? teamId)
        {
            _baseTeam = await _context.Team.FindAsync(teamId);
            if (_baseTeam is null)
                throw new ValidationException("Team with provided id does not exist");
        }

        private ChatProfile CreateUserChatProfile(string userId, Chat chat) =>
            new ChatProfile()
            {
                Chat = chat,
                TeamProfile = _baseTeam?.Profiles.FirstOrDefault(profile => profile.UserId == userId),
                ProjectProfile = _baseProject?.Profiles.FirstOrDefault(profile => profile.User.Id == userId),
            };
    }
}
