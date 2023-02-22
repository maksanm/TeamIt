using MediatR;

namespace Models.Chats.Commands
{
    public class LeaveChatCommand : IRequest
    {
        public long ChatId { get;set; }
    }
}
