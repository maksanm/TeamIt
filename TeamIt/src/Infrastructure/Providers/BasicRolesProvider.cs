using Application.Common.Interfaces;
using Application.Common.Providers;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Providers
{
    public class BasicRolesProvider : IBasicRolesProvider
    {
        private readonly IPermissionsProvider _permissionsProvider;

        public BasicRolesProvider(IPermissionsProvider permissionsProvider)
        {
            _permissionsProvider = permissionsProvider;
        }

        public Role TeamCreatorRole() => 
            new Role()
            {
                Name = "Team creator",
                Permissions = _permissionsProvider.AllPermissions()
            };

        public Role GuestRole() =>
            new Role()
            {
                Name = "Guest",
                Permissions = new List<Permission>()
            };
    }
}