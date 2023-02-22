using Domain.Entities;

namespace Application.Common.Providers
{
    public interface IBasicRolesProvider
    {
        Role TeamCreatorRole();

        Role GuestRole();
    }
}