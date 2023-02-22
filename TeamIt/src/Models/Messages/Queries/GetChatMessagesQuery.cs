using MediatR;
using Models.Messages.Dto;

namespace Models.Messages.Queries
{
    public class GetChatMessagesQuery : IRequest<IList<MessageDto>>
    {
        public long ChatId { get;set; }
        public int Limit { get; set; }
    }
}
