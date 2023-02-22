using Application.Common.Interfaces;
using Domain.Entities.Chats;
using Domain.Entities.ProjectManager;
using MediatR;
using Models.Chats.Commands;
using Domain.Enums;
using Application.Common.Exceptions;

namespace Application.Handlers.Chats.Commands
{
    public class DeleteChatCommandHandler : IRequestHandler<DeleteChatCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPermissionValidator _permissionValidator;
        private readonly IIdentityService _identityService;

        private Chat? _chat;

        public DeleteChatCommandHandler(
            IApplicationDbContext context,
            IPermissionValidator permissionValidator,
            IIdentityService identityService)
        {
            _context = context;
            _permissionValidator = permissionValidator;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(DeleteChatCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            if (request.ValidatePermissions)
                await _permissionValidator.ValidateChatPermission(request.ChatId, PermissionEnum.CHAT_DELETE);

            ClearChatRelatedEntities();
            _context.Chat.Remove(_chat!);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async System.Threading.Tasks.Task ValidateRequest(DeleteChatCommand request)
        {
            _chat = await _context.Chat.FindAsync(request.ChatId);
            if (_chat is null)
                throw new ValidationException("Chat with provided id does not exist");
        }

        private void ClearChatRelatedEntities()
        {
            _context.Message.RemoveRange(_chat!.Messages);
            _context.ChatProfile.RemoveRange(_chat.Profiles);
        }
    }
}
