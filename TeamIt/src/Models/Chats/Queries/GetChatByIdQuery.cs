using MediatR;
using Models.Chats.Dto;

namespace Models.Chats.Queries
{
    public class GetChatByIdQuery : IRequest<ChatDto>
    {
        public long ChatId { get; set; }
    }
}
