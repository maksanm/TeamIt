using MediatR;
using Models.Chats.Dto;

namespace Models.Chats.Queries
{
    public class GetCurrentUserChatInfosQuery : IRequest<IList<ChatInfoDto>>
    {
    }
}
