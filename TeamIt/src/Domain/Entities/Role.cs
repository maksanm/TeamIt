using Domain.Entities.Teams;

namespace Domain.Entities
{
    public class Role
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public virtual IList<Permission> Permissions { get; set; }
        public virtual IList<Team> Teams { get; set; }
    }
}