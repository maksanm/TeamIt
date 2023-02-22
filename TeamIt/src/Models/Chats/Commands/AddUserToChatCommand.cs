using MediatR;

namespace Models.Chats.Commands
{
    public class AddUserToChatCommand : IRequest
    {
        public long ChatId { get; set; }
        public string UserId { get; set; }
    }
}
