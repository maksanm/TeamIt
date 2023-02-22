using MediatR;
using Models.Permissions.Dto;

namespace Models.Chats.Queries
{
    public class GetCurrentUserChatPermissionsQuery : IRequest<IList<PermissionDto>>
    {
        public long ChatId { get; set; }
    }
}
