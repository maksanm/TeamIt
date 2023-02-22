using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Hubs;
using Domain.Entities.Chats;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Models.Messages.Commands;

namespace Application.Handlers.Messages.Commands
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionValidator _permissionValidator;
        private readonly IImageService _imageService;
        private readonly IHubContext<ChatHub, IChatHubClient> _chatHubContext;

        private Chat? _chat;
        private ChatProfile? _senderChatProfile = null;

        public SendMessageCommandHandler(
            IApplicationDbContext context,
            IPermissionValidator permissionValidator,
            IImageService imageService,
            IHubContext<ChatHub, IChatHubClient> chatHubContext)
        {
            _context = context;
            _permissionValidator = permissionValidator;
            _imageService = imageService;
            _chatHubContext = chatHubContext;
        }

        public async Task<Unit> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            if (request.AttachedImage is not null)
                await _permissionValidator.ValidateChatPermission(request.ChatId, PermissionEnum.CHAT_SEND_IMAGE);

            var message = new Message()
            {
                Chat = _chat!,
                SenderProfile = _senderChatProfile, //null if not provided - notification
                Text = request.Text,
                Date = DateTime.Now
            };
            _context.Message.Add(message);
            await _context.SaveChangesAsync(cancellationToken);

            if (request.AttachedImage is not null)
                await _imageService.AddMessageImage(message.Id, request.AttachedImage);

            await BroadcastMessageToChatMembers(message);
            return Unit.Value;
        }

        private async Task BroadcastMessageToChatMembers(Message message) =>
            await _chatHubContext.Clients
                .Group(_chat!.Id.ToString())
                .ReceiveMessage(_chat.Id, _senderChatProfile?.User.Id, message.Text, message.AttachedImage?.ImagePath);

        private async Task ValidateRequest(SendMessageCommand request)
        {
            await ValidateChat(request.ChatId);
            if (!string.IsNullOrEmpty(request.SenderUserId))
                ValidateSenderUser(request.SenderUserId);
            ValidateMessage(request);
        }

        private async Task ValidateChat(long chatId)
        {
            _chat = await _context.Chat.FindAsync(chatId);
            if (_chat is null)
                throw new ValidationException("Chat with provided id does not exist");
        }

        private void ValidateSenderUser(string senderUserId)
        {
            _senderChatProfile = _chat!.Profiles
                .FirstOrDefault(profile => profile.ChatId == _chat!.Id
                                        && profile.User.Id == senderUserId);
            if (_senderChatProfile is null)
                throw new ValidationException("User with provided id is not a member of the chat");
        }

        private void ValidateMessage(SendMessageCommand request)
        {
            if (string.IsNullOrEmpty(request.Text) && request.AttachedImage is null)
                throw new ValidationException("Message text cannot be empty If no image was attached");
        }

        private List<string> ChatMemberIds() =>
            _chat!.Profiles.Select(profile => profile.User.Id).ToList();
    }
}
