using MediatR;
using Models.Chats.Dto;
using Models.Users.Dto;

namespace Models.Chats.Queries
{
    public class GetUsersToChatWithQuery : IRequest<UsersToChatWithDto>
    {
    }
}
