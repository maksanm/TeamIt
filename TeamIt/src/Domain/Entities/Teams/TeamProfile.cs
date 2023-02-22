using Domain.Entities.Chats;
using Domain.Entities.ProjectManager;

namespace Domain.Entities.Teams
{
    public class TeamProfile : IProfile
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public long TeamId { get; set; }
        public virtual Team Team { get; set; }
        public long RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual IList<ProjectProfile> ProjectProfiles { get; set; }
        public virtual IList<ChatProfile> ChatProfiles { get; set; }
    }
}