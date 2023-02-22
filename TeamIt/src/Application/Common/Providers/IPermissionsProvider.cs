using Domain.Entities;

namespace Application.Common.Providers
{
    public interface IPermissionsProvider
    {
        List<Permission> PermissionsWithIds(List<int> ids);

        List<Permission> AllPermissions();
    }
}