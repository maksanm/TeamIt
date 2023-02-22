using Application.Common.Interfaces;
using Domain.Entities.Chats;
using MediatR;
using Models.Chats.Commands;
using Application.Common.Exceptions;
using Domain.Enums;

namespace Application.Handlers.Chats.Commands
{
    public class EditChatCommandHandler : IRequestHandler<EditChatCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionValidator _permissionValidator;
        private readonly IImageService _imageService;

        private Chat? _chat;

        public EditChatCommandHandler(
            IApplicationDbContext context,
            IPermissionValidator permissionValidator,
            IImageService imageService)
        {
            _context = context;
            _permissionValidator = permissionValidator;
            _imageService = imageService;
        }

        public async Task<Unit> Handle(EditChatCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            await _permissionValidator.ValidateChatPermission(request.ChatId, PermissionEnum.CHAT_EDIT);

            _chat!.Name = request.Name ?? _chat.Name;
            await _context.SaveChangesAsync(cancellationToken);
            if (request.ChatPicture is not null)
                await _imageService.SetChatPicture(_chat.Id, request.ChatPicture);
            return Unit.Value;
        }

        private async Task ValidateRequest(EditChatCommand request)
        {
            await ValidateChat(request.ChatId);
            ValidateChatName(request.Name);
        }

        private async Task ValidateChat(long? chatId)
        {
            _chat = await _context.Chat.FindAsync(chatId);
            if (_chat is null)
                throw new ValidationException("Chat with provided id does not exist");
        }

        private void ValidateChatName(string? name)
        {
            if (name is not null && string.IsNullOrWhiteSpace(name))
                throw new ValidationException("Chat name must be provided");
        }
    }
}
