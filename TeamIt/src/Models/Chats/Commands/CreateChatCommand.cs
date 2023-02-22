using MediatR;
using Microsoft.AspNetCore.Http;

namespace Models.Chats.Commands
{
    public class CreateChatCommand : IRequest
    {
        public string UserId { get; set; }
        public long? TeamId { get; set; }
        public long? ProjectId { get; set; }
        public string Name { get; set; }
        public IFormFile? ChatPicture { get; set; }
    }
}
