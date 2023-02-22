using Domain.Enums;

namespace Domain.Entities
{
    public class Permission
    {
        public PermissionEnum Id { get; set; }
        public string Name { get; set; }
        public virtual IList<Role> Roles { get; set; }
    }
}