using Application.Common.Interfaces;
using Domain.Entities.Chats;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Models.Chats.Commands;
using Application.Common.Exceptions;

namespace Application.Handlers.Chats.Commands
{
    public class LeaveChatCommandHandler : IRequestHandler<LeaveChatCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        private Chat? _chat;

        public LeaveChatCommandHandler(
            IApplicationDbContext context,
            IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(LeaveChatCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);

            var currentUserChatProfile = await _identityService.GetCurrentUserChatProfileAsync(request.ChatId);

            _context.ChatProfile.Remove(currentUserChatProfile);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async Task ValidateRequest(LeaveChatCommand request)
        {
            _chat = await _context.Chat.FindAsync(request.ChatId);
            if (_chat is null)
                throw new ValidationException("Chat with provided id does not exist");
        }
    }
}
