using MediatR;

namespace Models.Chats.Commands
{
    public class DeleteChatCommand : IRequest
    {
        public long ChatId { get; set; }
        public bool ValidatePermissions { get; set; } = true;
    }
}
