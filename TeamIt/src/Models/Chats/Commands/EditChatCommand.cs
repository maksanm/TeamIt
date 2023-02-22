using MediatR;
using Microsoft.AspNetCore.Http;

namespace Models.Chats.Commands
{
    public class EditChatCommand : IRequest
    {
        public long ChatId { get; set; }
        public string? Name { get; set; }
        public IFormFile? ChatPicture { get; set; }
    }
}
