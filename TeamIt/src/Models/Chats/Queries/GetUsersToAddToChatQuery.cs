using MediatR;
using Models.Chats.Dto;
using Models.Users.Dto;

namespace Models.Chats.Queries
{
    public class GetUsersToAddToChatQuery : IRequest<UsersToChatWithDto>
    {
        public long ChatId { get; set; }
    }
}
