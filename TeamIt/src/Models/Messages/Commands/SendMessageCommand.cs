using MediatR;
using Microsoft.AspNetCore.Http;

namespace Models.Messages.Commands
{
    public class SendMessageCommand : IRequest
    {
        public long ChatId { get; set; }
        public string? SenderUserId { get; set; }
        public string? Text { get; set; }
        public IFormFile? AttachedImage { get; set; } 
    }
}
