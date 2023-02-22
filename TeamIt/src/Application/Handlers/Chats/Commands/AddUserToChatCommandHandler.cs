using Application.Common.Interfaces;
using Domain.Entities.Chats;
using Domain.Entities.ProjectManager;
using Domain.Entities;
using MediatR;
using Models.Chats.Commands;
using Domain.Enums;
using Application.Common.Exceptions;

namespace Application.Handlers.Chats.Commands
{
    public class AddUserToChatCommandHandler : IRequestHandler<AddUserToChatCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionValidator _permissionValidator;

        private Chat? _chat;
        private User? _userToAdd;

        public AddUserToChatCommandHandler(
            IApplicationDbContext context,
            IPermissionValidator permissionValidator)
        {
            _context = context;
            _permissionValidator = permissionValidator;
        }

        public async Task<Unit> Handle(AddUserToChatCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            await _permissionValidator.ValidateChatPermission(request.ChatId, PermissionEnum.CHAT_ADD_USER);

            var newChatProfile = CreateUserChatProfile();
            _chat!.Profiles.Add(newChatProfile);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async System.Threading.Tasks.Task ValidateRequest(AddUserToChatCommand request)
        {
            await ValidateChat(request.ChatId);
            ValidateUser(request.UserId);
        }

        private ChatProfile CreateUserChatProfile() =>
            new ChatProfile()
            {
                Chat = _chat!,
                TeamProfile = _chat!.BaseTeam?.Profiles.FirstOrDefault(profile => profile.UserId == _userToAdd!.Id),
                ProjectProfile = _chat.BaseProject?.Profiles.FirstOrDefault(profile => profile.User.Id == _userToAdd!.Id),
            };

        private async System.Threading.Tasks.Task ValidateChat(long? chatId)
        {
            _chat = await _context.Chat.FindAsync(chatId);
            if (_chat is null)
                throw new ValidationException("Chat with provided id does not exist");
        }

        private void ValidateUser(string userId)
        {
            if (_chat!.BaseTeam is not null)
                _userToAdd = _chat.BaseTeam.Profiles
                    .Select(profile => profile.User)
                    .FirstOrDefault(user => user.Id == userId);
            else if (_chat.BaseProject is not null)
                _userToAdd = _chat.BaseProject.Profiles
                    .Select(profile => profile.User)
                    .FirstOrDefault(user => user.Id == userId);

            if (_userToAdd == default)
                throw new ValidationException("User with provided id is not a member of the chat base team or project");
        }
    }
}
