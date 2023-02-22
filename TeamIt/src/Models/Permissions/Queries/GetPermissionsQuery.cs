using MediatR;
using Models.Permissions.Dto;

namespace Models.Permissions.Queries
{
    public class GetPermissionsQuery : IRequest<IList<PermissionDto>>
    {
    }
}